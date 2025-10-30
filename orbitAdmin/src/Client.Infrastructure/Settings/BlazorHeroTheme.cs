using MudBlazor;

namespace SchoolV01.Client.Infrastructure.Settings
{
    public class BlazorHeroTheme
    {
        private readonly static Typography DefaultTypography = new()
        {

            Default = new DefaultTypography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            H1 = new H1Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "6rem",
                FontWeight = "300",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em"
            },
            H2 = new H2Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "3.75rem",
                FontWeight = "300",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em"
            },
            H3 = new H3Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "3rem",
                FontWeight = "400",
                LineHeight = "1.167",
                LetterSpacing = "0"
            },
            H4 = new H4Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "2.125rem",
                FontWeight = "400",
                LineHeight = "1.235",
                LetterSpacing = ".00735em"
            },
            H5 = new H5Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1.5rem",
                FontWeight = "400",
                LineHeight = "1.334",
                LetterSpacing = "0"
            },
            H6 = new H6Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1.25rem",
                FontWeight = "400",
                LineHeight = "1.6",
                LetterSpacing = ".0075em"
            },
            Button = new ButtonTypography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.75",
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            Caption = new CaptionTypography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".75rem",
                FontWeight = "400",
                LineHeight = "1.66",
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2Typography()
            {
                FontFamily = ["Montserrat", "Tajawal", "Helvetica", "Arial", "sans-serif"],
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.57",
                LetterSpacing = ".00714em"
            }
        };

        private static readonly LayoutProperties DefaultLayoutProperties = new()
        {
            DefaultBorderRadius = "3px",
            AppbarHeight = "80px"            
        };

        public static readonly MudTheme DefaultTheme = new()
        {

            PaletteLight = new PaletteLight()
            {
                Primary = "#7f58af",
                Secondary = "#64c5eb",
                Tertiary = "#e84d8a",
                AppbarBackground = "#7f58af",
                AppbarText = "#FFFFFF",
                Background = Colors.Gray.Lighten5,
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                Success = "#04AA6F",
                //Primary = "#08096c",
                //Secondary = "#32333d",
                //Tertiary = "#7B68EE",
                //AppbarBackground = "#08096c",
                //AppbarText = "#ECEFF1",
                //Background = Colors.Grey.Lighten5,
                //DrawerBackground = "#FFF",
                //DrawerText = "rgba(0,0,0, 0.7)",
                //Success = "#04AA6F"

                //Surface= "#ECEFF1"
                //  TextPrimary= "#ECEFF1"

            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#28aade",
                Secondary = "#EEEEEE",
                Tertiary = "#7B68EE",
                Success = "#007E33",
                Black = "#ffffff",
                Background = "#32333d",
                BackgroundGray = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#28aade",

                HoverOpacity = 1,
                AppbarText = "#373740",
                TextPrimary = "#F5F5F5",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#EEEEEE",
                ActionDisabled = "#757575",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                TextDisabled = "#757575",
                PrimaryContrastText = "#424242",
                SecondaryContrastText = "#424242"
            },

            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties,
            ZIndex = new ZIndex
            {
                Popover = 1400
            }
        };

      
    }
}