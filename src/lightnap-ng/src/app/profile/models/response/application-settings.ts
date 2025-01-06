import { ExtendedSettings, FeaturesSettings, PreferencesSettings } from "../settings";
import { LayoutConfig } from "../settings/layout-config";

/**
 * Represents the settings configuration for this app.
 */
export interface ApplicationSettings {
    /**
     * Extended settings for the app not covered by other settings.
     */
    extended: ExtendedSettings;

    /**
     * Feature-specific settings for the app.
     */
    features: FeaturesSettings;

    /**
     * User preferences settings for the app.
     */
    preferences: PreferencesSettings;

    /**
     * Style-related settings for the app.
     */
    style: LayoutConfig;
}
