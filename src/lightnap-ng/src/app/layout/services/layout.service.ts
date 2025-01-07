import { Injectable, signal, computed, effect, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { APP_NAME } from "@core";
import { IdentityService } from "@identity";
import { LayoutState } from "@layout/models/layout-state";
import { LayoutConfig, ProfileService } from "@profile";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class LayoutService {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  appName = inject(APP_NAME);

  #config = this.#profileService.getDefaultStyleSettings();

  #state: LayoutState = {
    staticMenuDesktopInactive: false,
    overlayMenuActive: false,
    configSidebarVisible: false,
    staticMenuMobileActive: false,
    menuHoverActive: false,
  };

  layoutConfig = signal<LayoutConfig>(this.#config);
  layoutState = signal<LayoutState>(this.#state);
  #configUpdate = new Subject<LayoutConfig>();
  #overlayOpen = new Subject<any>();
  configUpdate$ = this.#configUpdate.asObservable();
  overlayOpen$ = this.#overlayOpen.asObservable();
  theme = computed(() => (this.layoutConfig()?.darkTheme ? "light" : "dark"));
  isSidebarActive = computed(() => this.layoutState().overlayMenuActive || this.layoutState().staticMenuMobileActive);
  isDarkTheme = computed(() => this.layoutConfig().darkTheme);
  getPrimary = computed(() => this.layoutConfig().primary);
  getSurface = computed(() => this.layoutConfig().surface);
  isOverlay = computed(() => this.layoutConfig().menuMode === "overlay");
  transitionComplete = signal<boolean>(false);

  #initialized = false;

  constructor() {
    effect(() => {
      const config = this.layoutConfig();
      if (config) {
        this.onConfigUpdate();
      }
    });

    effect(() => {
      const config = this.layoutConfig();

      if (!this.#initialized || !config) {
        this.#initialized = true;
        return;
      }

      this.handleDarkModeTransition(config);
    });

    this.#identityService
      .watchLoggedIn$()
      .pipe(takeUntilDestroyed())
      .subscribe(loggedIn => {
        if (loggedIn) {
          this.#profileService.getSettings().subscribe(settings => this.layoutConfig.set(settings.style));
        } else {
          this.layoutConfig.set(this.#profileService.getDefaultStyleSettings());
        }
      });
  }

  private handleDarkModeTransition(config: LayoutConfig): void {
    if ((document as any).startViewTransition) {
      this.startViewTransition(config);
    } else {
      this.toggleDarkMode(config);
      this.#onTransitionEnd();
    }
  }

  private startViewTransition(config: LayoutConfig): void {
    const transition = (document as any).startViewTransition(() => {
      this.toggleDarkMode(config);
    });

    transition.ready.then(() => this.#onTransitionEnd()).catch(() => {});
  }

  toggleDarkMode(config?: LayoutConfig): void {
    const _config = config || this.layoutConfig();
    if (_config.darkTheme) {
      document.documentElement.classList.add("app-dark");
    } else {
      document.documentElement.classList.remove("app-dark");
    }
  }

  #onTransitionEnd() {
    this.transitionComplete.set(true);
    setTimeout(() => {
      this.transitionComplete.set(false);
    });
  }

  onMenuToggle() {
    if (this.isOverlay()) {
      this.layoutState.update(prev => ({ ...prev, overlayMenuActive: !this.layoutState().overlayMenuActive }));

      if (this.layoutState().overlayMenuActive) {
        this.#overlayOpen.next(null);
      }
    }

    if (this.isDesktop()) {
      this.layoutState.update(prev => ({ ...prev, staticMenuDesktopInactive: !this.layoutState().staticMenuDesktopInactive }));
    } else {
      this.layoutState.update(prev => ({ ...prev, staticMenuMobileActive: !this.layoutState().staticMenuMobileActive }));

      if (this.layoutState().staticMenuMobileActive) {
        this.#overlayOpen.next(null);
      }
    }
  }

  isDesktop() {
    return window.innerWidth > 991;
  }

  isMobile() {
    return !this.isDesktop();
  }

  onConfigUpdate() {
    this.#config = { ...this.layoutConfig() };
    this.#configUpdate.next(this.layoutConfig());

    if (this.#profileService.hasLoadedStyleSettings()) {
      this.#profileService.updateStyleSettings({ ...this.layoutConfig() }).subscribe({
        error: response => console.error("Unable to save settings", response.errorMessages),
      });
    }
  }
}
