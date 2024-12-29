import { effect, inject, Injectable, signal } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { StyleSettings } from "@profile";
import { ProfileService } from "@profile/services/profile.service";
import { Subject } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { LayoutState } from "../models/layout-state";
import { APP_NAME } from "@core";

@Injectable({
  providedIn: "root",
})
export class LayoutService {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  appName = inject(APP_NAME);

  #styleSettings = this.#profileService.getDefaultStyleSettings();

  config = signal<StyleSettings>(this.#styleSettings);

  state: LayoutState = {
    staticMenuDesktopInactive: false,
    overlayMenuActive: false,
    profileSidebarVisible: false,
    configSidebarVisible: false,
    staticMenuMobileActive: false,
    menuHoverActive: false,
  };

  #configUpdate = new Subject<StyleSettings>();
  #overlayOpen = new Subject<any>();

  configUpdate$ = this.#configUpdate.asObservable();
  overlayOpen$ = this.#overlayOpen.asObservable();

  constructor() {
    effect(() => {
      const config = this.config();
      if (this.themeIsChanged(config)) {
        this.changeTheme();
      }
      this.changeScale(config.scale);
      this.onConfigUpdate();
    });

    this.#identityService
      .watchLoggedIn$()
      .pipe(takeUntilDestroyed())
      .subscribe(loggedIn => {
        if (loggedIn) {
          this.#profileService.getSettings().subscribe(settings => this.config.set(settings.style));
        } else {
          this.config.set(this.#profileService.getDefaultStyleSettings());
        }
      });
  }

  themeIsChanged(styleSettings: StyleSettings) {
    return styleSettings.theme !== this.#styleSettings.theme || styleSettings.colorScheme !== this.#styleSettings.colorScheme;
  }

  onMenuToggle() {
    if (this.isOverlay()) {
      this.state.overlayMenuActive = !this.state.overlayMenuActive;
      if (this.state.overlayMenuActive) {
        this.#overlayOpen.next(null);
      }
    }

    if (this.isDesktop()) {
      this.state.staticMenuDesktopInactive = !this.state.staticMenuDesktopInactive;
    } else {
      this.state.staticMenuMobileActive = !this.state.staticMenuMobileActive;

      if (this.state.staticMenuMobileActive) {
        this.#overlayOpen.next(null);
      }
    }
  }

  showProfileSidebar() {
    this.state.profileSidebarVisible = !this.state.profileSidebarVisible;
    if (this.state.profileSidebarVisible) {
      this.#overlayOpen.next(null);
    }
  }

  showConfigSidebar() {
    this.state.configSidebarVisible = true;
  }

  isOverlay() {
    return this.config().menuMode === "overlay";
  }

  isDesktop() {
    return window.innerWidth > 991;
  }

  isMobile() {
    return !this.isDesktop();
  }

  onConfigUpdate() {
    this.#styleSettings = { ...this.config() };
    this.#configUpdate.next(this.config());

    if (this.#profileService.hasLoadedStyleSettings()) {
      this.#profileService.updateStyleSettings(this.#styleSettings).subscribe({
        error: response => console.error("Unable to save settings", response.errorMessages),
      });
    }
  }

  changeTheme() {
    const config = this.config();
    const themeLink = <HTMLLinkElement>document.getElementById("theme-css");
    const themeLinkHref = themeLink.getAttribute("href")!;
    const newHref = themeLinkHref
      .split("/")
      .map(el =>
        el == this.#styleSettings.theme
          ? (el = config.theme)
          : el == `theme-${this.#styleSettings.colorScheme}`
          ? (el = `theme-${config.colorScheme}`)
          : el
      )
      .join("/");

    this.replaceThemeLink(newHref);
  }
  replaceThemeLink(href: string) {
    const id = "theme-css";
    let themeLink = <HTMLLinkElement>document.getElementById(id);
    const cloneLinkElement = <HTMLLinkElement>themeLink.cloneNode(true);

    cloneLinkElement.setAttribute("href", href);
    cloneLinkElement.setAttribute("id", id + "-clone");

    themeLink.parentNode!.insertBefore(cloneLinkElement, themeLink.nextSibling);
    cloneLinkElement.addEventListener("load", () => {
      themeLink.remove();
      cloneLinkElement.setAttribute("id", id);
    });
  }

  changeScale(value: number) {
    document.documentElement.style.fontSize = `${value}px`;
  }
}
