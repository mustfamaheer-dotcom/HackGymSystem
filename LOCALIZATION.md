# Localization Guide

## Overview

This system uses ASP.NET Core's built-in localization framework with `.resx` resource files. Arabic (`ar`) is the default language, with English (`en`) as fallback. The infrastructure is designed to support additional languages with minimal effort.

## How It Works

### Localization Pipeline

1. **Request Localization Middleware** (`Program.cs:156`) — intercepts every request, determines the user's preferred culture from:
   - Cookie (`.AspNetCore.Culture`) — set by the language switcher
   - User profile (future enhancement: persist to DB)
   - Browser's `Accept-Language` header (last resort)
   
2. **Default Culture**: Arabic (`ar`) — configured in `Program.cs:148-155`

3. **Resource Resolution**: When a view calls `@SharedLocalizer["Home"]`:
   - If current culture is `ar` → looks in `Resources/SharedResources.ar.resx` for key "Home"
   - If not found → falls back to `Resources/SharedResources.resx` (English)
   - If still not found → returns the key itself (`"Home"`)

4. **RTL/LTR Switching** (`_Layout.cshtml:4-6`): The `<html>` tag dynamically sets `dir="rtl"` or `dir="ltr"` and `lang="ar"` or `lang="en"` based on `CultureInfo.CurrentUICulture`

### File Structure

```
src/Gym.API/
├── Resources/
│   ├── SharedResources.cs          # Marker class (required for DI)
│   ├── SharedResources.resx        # English resource file (fallback)
│   └── SharedResources.ar.resx     # Arabic translations (342 entries)
├── Controllers/
│   └── AccountController.cs        # SetLanguage action at line 73
├── Program.cs                      # Localization services + middleware
└── Views/
    ├── _ViewImports.cshtml         # @inject IViewLocalizer + IStringLocalizer<SharedResources>
    └── Shared/_Layout.cshtml       # dir/lang attributes, RTL CSS, language switcher
```

### Language Switcher

- Located in `_Layout.cshtml` topbar, next to the theme toggle
- Displays "EN" when Arabic is active, "AR" when English is active
- Posts to `/Account/SetLanguage` with `culture` and `returnUrl` parameters
- The `SetLanguage` action (AccountController.cs) sets the `.AspNetCore.Culture` cookie

## Adding New Translation Keys

### Step 1: Add the Key to Views

Replace hardcoded text in `.cshtml` files with `@SharedLocalizer["KeyName"]`:

```razor
@* Before *@
<h4>Total Members</h4>

@* After *@
<h4>@SharedLocalizer["Total Members"]</h4>
```

Use the **exact English text** as the key name. This ensures English works as fallback without a resource entry.

### Step 2: Add Arabic Translation

Open `Resources/SharedResources.ar.resx` and add the key-value pair. You can use any text editor, Visual Studio's resource editor, or the provided script.

### Step 3: Verify

Run the application, switch to Arabic, and confirm the translation appears.

## Adding a New Language (e.g., French)

### Step 1: Register the Culture

In `Program.cs`, add the new culture to the `supportedCultures` array:

```csharp
var supportedCultures = new[]
{
    new CultureInfo("ar"),
    new CultureInfo("en"),
    new CultureInfo("fr")   // Add this line
};
```

### Step 2: Create Resource File

Create `Resources/SharedResources.fr.resx` with French translations. Use the same keys as the existing resource files.

The simplest way: copy `SharedResources.ar.resx` and replace the Arabic values with French translations.

### Step 3: Update Language Switcher

The language switcher in `_Layout.cshtml` currently toggles between `ar` and `en`. To support multiple languages, replace the simple toggle with a dropdown:

```razor
<select onchange="window.location.href='/Account/SetLanguage?culture='+this.value+'&returnUrl=@Context.Request.Path'">
    <option value="ar" selected="@(currentCulture.StartsWith("ar"))">العربية</option>
    <option value="en" selected="@(currentCulture.StartsWith("en"))">English</option>
    <option value="fr" selected="@(currentCulture.StartsWith("fr"))">Français</option>
</select>
```

### Step 4: Add RTL/LTR Support

- If the new language is RTL (e.g., Arabic, Urdu, Hebrew) → no action needed, existing RTL CSS handles it
- If the new language is LTR → no action needed, default CSS handles it
- Add any language-specific font imports in `_Layout.cshtml`

## Best Practices

### For Developers

1. **Never hardcode visible text** — always use `@SharedLocalizer["key"]`
2. **Use English text as keys** — this makes the view readable without a resource file
3. **Keep keys consistent** — reuse existing keys rather than creating synonyms
4. **Review `SharedResources.ar.resx`** when adding new features to check if keys already exist
5. **Add keys to both resource files** — English `.resx` for documentation, Arabic `.resx` for translation

### Technical Notes

- `IViewLocalizer` (`Localizer`) is available for view-specific resources
- `IStringLocalizer<SharedResources>` (`SharedLocalizer`) is available for shared resources across all views
- Both support string formatting: `@SharedLocalizer["Welcome {0}", username]`
- Resource files support `{0}`, `{1}`, etc. placeholders for dynamic content
- The `SetLanguage` action in `AccountController.cs` can be extended to persist language preference to the user's database profile for authenticated users
- For API controllers, inject `IStringLocalizer<SharedResources>` to return localized error messages

### Testing Localization

1. Navigate to any page while logged in
2. Click the language switcher (AR/EN button in the topbar)
3. Verify the interface flips between RTL (Arabic) and LTR (English)
4. Check that dates, numbers, and all UI text are translated
5. Verify the Login page also switches language (though it stays light-mode only)
6. Test form submission, validation errors, and table pagination in both languages
