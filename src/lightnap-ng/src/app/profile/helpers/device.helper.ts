import { Device } from "@profile/models";

export class DeviceHelper {
    public static rehydrate(device: Device) {
        if (device?.lastSeen) {
            device.lastSeen = new Date(device.lastSeen);
        }
    }
}
