using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PlatformLibrary;
namespace ZunePlatform
{
    class windowAsControl : Control
    {
        public ChildChangedEventArgs OnControlAdded;
        public ChildChangedEventArgs OnControlRemoved;
        public windowAsControl()
        {
            onAdd += new ChildChangedEventArgs(windowAsControl_onAdd);
            onRemove += new ChildChangedEventArgs(windowAsControl_onRemove);
        }

        void windowAsControl_onRemove(Control child)
        {
            OnControlRemoved.Invoke(child);
        }

        void windowAsControl_onAdd(Control child)
        {
            OnControlAdded.Invoke(child);
        }

    }
    public class Window : IWindow
    {
        public Window()
        {
            nativeBinding = new System.Windows.Forms.Form();
            onShown += new DWMEventArgs(Window_onShown);
            onHide += new DWMEventArgs(Window_onHide);
            onFocus += new DWMEventArgs(Window_onFocus);
            Children = new childContainer(interntroll);

        }
        windowAsControl interntroll = new windowAsControl();
        public childContainer Children;
        void Window_onFocus()
        {

            ((System.Windows.Forms.Form)nativeBinding).Focus();
        }

        void Window_onHide()
        {
            ((System.Windows.Forms.Form)nativeBinding).Hide();
        }

        void Window_onShown()
        {
            ((System.Windows.Forms.Form)nativeBinding).Show();

        }

    }
    public class WindowManager : IWindowManager
    {
        public WindowManager()
        {
            IWindowManager.Default = this;
            onInit += new DWMInitializeArgs(WindowManager_onInit);
        }
        static bool isWindowManagerStarted = false;
        public override void CreateWindow(IWindow window)
        {
            //Windows will NOT be rendered with WPF to look nice (because CE doesn't support WPF!)

            base.CreateWindow(window);
        }
        void WindowManager_onInit()
        {
            if (!isWindowManagerStarted)
            {
                isWindowManagerStarted = true;

            }
        }
    }
    public class Button : IButton
    {

        public Button()
        {
            onAdd += new ChildChangedEventArgs(Button_onAdd);
            onRemove += new ChildChangedEventArgs(Button_onRemove);
        }

        void Button_onAdd(Control child)
        {
            throw new NotImplementedException();
        }

        void Button_onRemove(Control child)
        {
            throw new NotImplementedException();
        }
    }
}
