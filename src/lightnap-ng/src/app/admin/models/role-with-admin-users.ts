import { AdminUser, Role } from "./response";

export interface RoleWithAdminUsers {
    role: Role;
    users: Array<AdminUser>;
}
