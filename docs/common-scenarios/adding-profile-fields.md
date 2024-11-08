---
title: Adding Profile Fields
layout: home
parent: Common Scenarios
nav_order: 400
---

# {{ page.title }}

The default LightNap profile isn't very interesting. It just has some default fields and timestamps. Let's take a look at how it can be extended to have new fields for first and last names.

- TOC
{:toc}

## Back-End Changes

We'll start off by updating the back-end by changing in layers from entity model to DTOs and then services. Most of the back-end work that needs to be done to change the data model happens in the `LightNap.Core` project.

### Updating the Entity

1. Open `Data/Entities/ApplicationUser.cs`. This is the entity that represents a user.
2. Add properties for the first and last name.

    ```csharp
    public class ApplicationUser : IdentityUser
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

3. Update the constructor to set default values for these fields. Alternatively you could allow these to be nullable or required as constructor parameters. This tutorial will keep things simple and just default them to values.

    ```csharp
    [SetsRequiredMembers]
    public ApplicationUser(string userName, string email, bool twoFactorEnabled)
    {
      this.FirstName = "DefaultFirst";
      this.LastName = "DefaultLast";
      ...
    ```

4. Add an [Entity Framework migration](../getting-started/database-providers/ef-migrations) and update the database.

    {: .note}
    It's recommended to use the [in-memory data provider](../getting-started//database-providers/in-memory-provider) while working out the details of an entity model update, if feasible. Then a single migration can be created and applied once the design is finalized.

### Updating the Data Transfer Objects (DTOs)

Almost all access to the `ApplicationUser` class is restricted to the services exposed by the project. As a result, there are DTOs that need to be updated.

1. Open `Profile/Dto/Response/ProfileDto.cs`. This is the DTO used in responses to logged-in user requests for their own profile.
2. Add fields for the first and last name.

    ```csharp
    public class ProfileDto
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

3. Open `Profile/Dto/Request/UpdateProfileDto.cs`. This is the DTO used by users requesting updates to their profile.
4. Add fields for the first and last name.

    ```csharp
    public class UpdateProfileDto
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

5. Open `Administrator/Dto/Response/AdminUserDto.cs`. This is the DTO used in responses to administrator requests for users.
6. Add fields for the first and last name.

    ```csharp
    public class AdminUserDto
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

7. Open `Administrator/Dto/Request/UpdateAdminUserDto.cs`. This is the DTO used by administrators requesting updates to a user.
8. Add fields for the first and last name.

    ```csharp
    public class UpdateAdminUserDto
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

{: .note}
If there were other DTOs for `ApplicationUser`, such as those used by the `PublicService` or `UserService` services, then those would need to be updated as well.

### Updating the Extension Method Mappings

There is no direct mapping relationship between the `ApplicationUser` class and its related DTOs. That mapping is all performed by external extension methods added to `ApplicationUser`. Those methods need to be updated to account for the new fields.

1. Open `Extensions/ApplicationUserExtensions.cs`. This class contains all extension methods for converting `ApplicationUser` instances to DTOs and for applying changes from DTOs to an `ApplicationUser` instance.
2. Add fields for the first and last name to the `ToLoggedInUserDto` method.

    ```csharp
    public static ProfileDto ToLoggedInUserDto(this ApplicationUser user)
    {
      return new ProfileDto()
      {
        FirstName = user.FirstName,
        LastName = user.LastName,
        ...
    ```

3. Add fields for the first and last name to the `UpdateLoggedInUser` method.

    ```csharp
    public static void UpdateLoggedInUser(this ApplicationUser user, UpdateProfileDto dto)
    {
      user.FirstName = dto.FirstName;
      user.LastName = dto.LastName;
      ...
    ```

4. Add fields for the first and last name to the `ToAdminUserDto` method.

    ```csharp
    public static AdminUserDto ToAdminUserDto(this ApplicationUser user)
    {
      ...
      return new AdminUserDto()
      {
        FirstName = user.FirstName,
        LastName = user.LastName,
        ...
    ```

5. Add fields for the first and last name to the `UpdateAdminUserDto` method.

    ```csharp
    public static void UpdateAdminUserDto(this ApplicationUser user, UpdateAdminUserDto dto)
    {
      user.FirstName = dto.FirstName;
      user.LastName = dto.LastName;
      ...
    ```

### Updating the Registration Back-End

In this scenario we will assume that the user also needs to provide these fields when registering a new account.

1. Open `Identity/Dto/Request/RegisterRequestDto.cs`. This is the DTO submitted by users registering an account on the site.
2. Add fields for the first and last name.

    ```csharp
    public class RegisterRequestDto
    {
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      ...
    ```

3. Open `Identity/Services/IdentityService.cs`. This is the service that fulfills all identity-related functionality.

4. Update the `RegisterAsync` method to set the fields on a new `ApplicationUser`.

    ```csharp
    public async Task<ApiResponseDto<LoginResultDto>> RegisterAsync(RegisterRequestDto requestDto)
    {
      ...
      ApplicationUser user = new(requestDto.UserName, requestDto.Email, applicationSettings.Value.RequireTwoFactorForNewUsers);
      user.FirstName = requestDto.FirstName;
      user.LastName = requestDto.LastName;
      ...
    ```

### Updating the Administrator Search Users Back-End

1. Open `Administrator/Dto/Request/SearchAdminUsersDto.cs`. This is the DTO used by administrators to search the membership across supported fields.
2. Add fields for the first and last name.

    ```csharp
    public class SearchAdminUsersDto
    {
      public string? FirstName { get; set; }
      public string? LastName { get; set; }
      ...
    ```

3. Open `Administrator/Services/AdministratorService.cs`. This is the service that fulfills all administrator-related functionality.
4. Update the `SearchUsersAsync` method to apply the name parameters for exact matches, if provided.

    ``` csharp
    public async Task<ApiResponseDto<PagedResponse<AdminUserDto>>> SearchUsersAsync(SearchUsersRequestDto requestDto)
    {
        IQueryable<ApplicationUser> query = db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(requestDto.FirstName))
        {
            query = query.Where(user => user.FirstName == user.FirstName);
        }

        if (!string.IsNullOrWhiteSpace(requestDto.LastName))
        {
            query = query.Where(user => user.LastName == user.LastName);
        }
    ...
    ```

### Additional Back-End Changes

Because all profile manipulation is handled through DTOs and extension methods there is no need to make any other changes on the back-end. The data will now flow from the REST API as request DTOs that validate input values as required.

If there is a need to enforce additional restrictions, such as length ranges, that can be done via attributes on the request DTOs (see `RegisterRequestDto` for examples on how this can be done). Otherwise all incoming request DTOs are passed by the controllers to their underlying services that call `ApplicationUser` extension methods to get or update database data. However, if there is a need to apply further rules or transformations, that can be done within the service methods.

## Front-End Changes

The front-end is also divided into areas that map directly to the back-end areas including profile, administrator, and identity. We will approach them area by area so that a full data flow from API to component can be completed before moving to the next. Everything front-end is contained in the `lightnap-ng` project.

### Updating the Registration Front-End

1. Open `app/identity/models/request/register-request.ts`. This is the model that maps to the back-end `RegisterRequestDto`.
2. Add fields for the first and last names.

    ``` typescript
    export interface RegisterRequest {
      firstName: string;
      lastName: string;
      ...
    ```

3. Open `app/identity/components/pages/register.component.ts`. This is the code for the page where users register.
4. Add fields for the first and last names to the form. This will allow easy binding in the reactive form markup.

    ``` typescript
    export class RegisterComponent {
      ...
      form = this.#fb.nonNullable.group({
        firstName: this.#fb.control("", [Validators.required]),
        lastName: this.#fb.control("", [Validators.required]),
      ...
    ```

5. Update the `#identityService.register()` parameter with fields for the names.

    ``` typescript
    register() {
      ...
      this.#identityService.register({
        firstName: this.form.value.firstName,
        lastName: this.form.value.lastName,
        ...
    ```

6. Open `app/identity/components/pages/register.component.html`. This is the markup for the page where users register.
7. Add input fields for the names before the password input.

    ``` html
    ...
    <label for="firstName" class="block text-900 text-xl font-medium mb-2">First Name</label>
    <input
      id="firstName"
      type="text"
      placeholder="First Name"
      pInputText
      formControlName="firstName"
      class="w-full md:w-30rem mb-5"
      style="padding: 1rem"
    />

    <label for="lastName" class="block text-900 text-xl font-medium mb-2">Last Name</label>
    <input
      id="lastName"
      type="text"
      placeholder="Last Name"
      pInputText
      formControlName="lastName"
      class="w-full md:w-30rem mb-5"
      style="padding: 1rem"
    />

    <label for="password" class="block text-900 font-medium text-xl mb-2">Password</label>
    ...
    ```

### Updating the Profile Front-End

1. Open `app/profile/models/response/profile.ts`. This is the model that maps to the back-end `ProfileDto`.
2. Add fields for the first and last names.

    ``` typescript
    export interface Profile {
      firstName: string;
      lastName: string;
      ...
    ```

3. Open `app/profile/models/request/update-profile-request.ts`. This is the model that maps to the back-end `UpdateProfileDto`.
4. Add fields for the first and last names.

    ``` typescript
    export interface UpdateProfileRequest {
      firstName: string;
      lastName: string;
      ...
    ```

5. Open `app/profile/components/pages/index.component.ts`. This is the code for the page users see when they visit their profile. It includes a stub for a profile update form, but there are no fields by default.
6. Add fields for the first and last names to the form. This will allow easy binding in the reactive form markup.

    ``` typescript
    export class RegisterComponent {
      ...
      form = this.#fb.group({
        firstName: this.#fb.control("", [Validators.required]),
        lastName: this.#fb.control("", [Validators.required]),
      ...
    ```

7. Update the `getProfile` `tap` to update the values in the form after the profile has loaded.

    ``` typescript
    profile$ = this.#profileService.getProfile().pipe(
      tap(response => {
        if (!response.result) return;
        // Set form values.
        this.form.setValue({
          firstName: response.result.firstName,
          lastName: response.result.lastName,
          ...
    ```

8. Update the call to the `#profileService.updateProfile` parameter to include the new fields.

    ``` typescript
    updateProfile() {
      ...
      this.#profileService.updateProfile({
        firstName: this.form.value.firstName,
        lastName: this.form.value.lastName
      ...
    ```

9. Open `app/profile/components/pages/index.component.html`. This is the markup for the page users see when they visit their profile. It also includes a stub for a profile update form, but there are no fields by default.
10. Update the body of the form with some markup for the new fields.

    ``` html
    <form [formGroup]="form" (ngSubmit)="updateProfile()" autocomplete="off">
    <div class="flex flex-column w-20rem">
      <label for="firstName" class="font-semibold mb-2">First Name</label>
      <input id="firstName" type="text" pInputText formControlName="firstName" class="w-full mb-2" style="padding: 1rem" />
      <label for="lastName" class="font-semibold mb-2">Last Name</label>
      <input id="lastName" type="text" pInputText formControlName="lastName" class="w-full mb-2" style="padding: 1rem" />
    ...
    ```

### Updating the Administrator Front-End

Updating the Administrator functionality is similar to the Profile work we just completed, so we'll skip most of it for brevity. But to illustrate the consistency of the areas, here are the general steps to add it:

1. Update `app/administrator/models/response/admin-user.ts` with the new fields. This is the model that maps to the back-end `AdminUserDto`.
2. Update `app/administrator/models/request/update-admin-user-request.ts` with the new fields. This is the model that maps to the back-end `UpdateAdminUserDto`.
3. Update `app/administrator/models/request/search-admin-users-request.ts` with the new fields. This is the model that maps to the back-end `SearchAdminUsersDto`.
4. Update `users.component.ts` and `users.component.html` from `app/administrator/components/pages/users` with form fields and markup to search users and/or include the new fields in the results table.
5. Update `user.component.ts` and `user.component.html` from `app/administrator/components/pages/user` with form fields and markup to view and update user fields.
