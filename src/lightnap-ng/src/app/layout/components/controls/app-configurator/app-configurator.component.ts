import { CommonModule, isPlatformBrowser } from "@angular/common";
import { Component, computed, inject, PLATFORM_ID, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { LayoutService } from "@layout/services/layout.service";
import { $t } from "@primeng/themes";
import { PrimeNG } from "primeng/config";
import { SelectButtonModule } from "primeng/selectbutton";

@Component({
  selector: "app-configurator",
  templateUrl: "./app-configurator.component.html",
  imports: [CommonModule, FormsModule, SelectButtonModule],
  host: {
    class:
      "hidden absolute top-[3.25rem] right-0 w-72 p-4 bg-surface-0 dark:bg-surface-900 border border-surface rounded-border origin-top shadow-[0px_3px_5px_rgba(0,0,0,0.02),0px_0px_2px_rgba(0,0,0,0.05),0px_1px_4px_rgba(0,0,0,0.08)]",
  },
})
export class AppConfiguratorComponent {
  readonly config = inject(PrimeNG);
  readonly router = inject(Router);
  readonly layoutService = inject(LayoutService);
  readonly platformId = inject(PLATFORM_ID);
  readonly primeng = inject(PrimeNG);
  readonly presets = Object.keys(this.layoutService.presets);
  readonly showMenuModeButton = signal(!this.router.url.includes("identity"));

  readonly surfaces = this.layoutService.surfaces;

  readonly menuModeOptions = [
    { label: "Static", value: "static" },
    { label: "Overlay", value: "overlay" },
  ];

  selectedPrimaryColor = computed(() => this.layoutService.layoutConfig().primary);
  selectedSurfaceColor = computed(() => this.layoutService.layoutConfig().surface);
  selectedPreset = computed(() => this.layoutService.layoutConfig().preset);
  menuMode = computed(() => this.layoutService.layoutConfig().menuMode);

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.onPresetChange(this.layoutService.layoutConfig().preset);
    }
  }

  updateColors(event: any, type: string, color: any) {
    if (type === "primary") {
      this.layoutService.layoutConfig.update(state => ({ ...state, primary: color.name }));
      this.layoutService.updatePreset();
    } else if (type === "surface") {
      this.layoutService.layoutConfig.update(state => ({ ...state, surface: color.name }));
      this.layoutService.updateSurfacePalette(color.palette);
    }
    event.stopPropagation();
  }

  onPresetChange(event: any) {
    this.layoutService.layoutConfig.update(state => ({ ...state, preset: event }));
    const preset = this.layoutService.presets[event as keyof typeof this.layoutService.presets];
    const surfacePalette = this.layoutService.surfaces.find(s => s.name === this.selectedSurfaceColor())?.palette;
    $t().preset(preset).preset(this.layoutService.getPresetExt()).surfacePalette(surfacePalette).use({ useDefaultOptions: true });
  }

  onMenuModeChange(event: string) {
    this.layoutService.layoutConfig.update(prev => ({ ...prev, menuMode: event }));
  }
}
