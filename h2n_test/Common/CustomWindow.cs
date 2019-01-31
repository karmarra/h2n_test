using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace h2n_test.Common
{
    public class CustomWindow : Window
    {
        private WindowInteropHelper _windowHelper;

        public CustomWindow():base()
        {
            _windowHelper = new WindowInteropHelper(this);
            ContentRendered += OnContentRendered;
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            EnableBlur();
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("MinButton") is Button minButton)
                minButton.Click += MinButtonOnClick;
            if (GetTemplateChild("MaxButton") is Button maxButton)
                maxButton.Click += MaxButtonOnClick;
            if (GetTemplateChild("CloseButton") is Button closeButton)
                closeButton.Click += CloseButtonOnClick;
            base.OnApplyTemplate();
        }

        private void MinButtonOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MaxButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
            }
            else
            {
                SystemCommands.MaximizeWindow(this);
            }
        }

        private void CloseButtonOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        internal enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4,
            ACCENT_ENABLE_ACRYLICBLURBEHIND
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }


        //[Flags]
        //internal enum DwmBlurBehindFlags : uint
        //{
        //    DWM_BB_ENABLE = 0x00000001,
        //    DWM_BB_BLURREGION = 0x00000002,
        //    DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
        //}


        //[StructLayout(LayoutKind.Sequential)]
        //internal struct DWM_BLURBEHIND
        //{
        //    public DwmBlurBehindFlags dwFlags;
        //    public bool fEnable;
        //    public IntPtr hRgnBlur;
        //    public bool fTransitionOnMaximized;
        //}


        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        //[DllImport("dwmapi.dll", PreserveSig = true)]
        //internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //internal static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);


        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);

            var currentVersion = Environment.OSVersion.Version;
            if (currentVersion.Major > 6)
                accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
             else if (currentVersion.Major == 6)
                accent.AccentState = currentVersion.Minor > 1 ? AccentState.ACCENT_ENABLE_BLURBEHIND : AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;

            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
            accent.AccentFlags = 2;
            accent.GradientColor = 0x00FFFFFF;  

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}
