using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.simulation.logiccontrolpattern
{
    public abstract class Attachable<TControl> : IAttachable<TControl> where TControl : Control
    {
        public Control mommyControl { get; set; }
        protected TControl control { get; set; }

        

        public Attachable()
        {
            InitTControl();
        }
        
        public Attachable(Control mommyControl, Point location)
        {
            InitTControl();
            
            this.Location = location;
            AttachTo(mommyControl);
        }

        
        
        protected abstract void InitTControl();

        public abstract void Init();


        
        public Size Size
        {
            get => GetControl().Size;
            set => GetControl().Size = value;
        }

        public Point Location
        {
            get => GetControl().Location;
            set => GetControl().Location = value;
        }
        
        public void UpdateSize()
        {
            Size = Utils.GetCorrectSize(GetControl());
        }

        
        
        public void AttachTo(Control mommyControl)
        {
            if (mommyControl == null)
            {
                return;
            }
            this.mommyControl = mommyControl;
            mommyControl.Controls.Add(GetControl());
        }

        public void DetachFrom()
        {
            mommyControl.Controls.Remove(GetControl());
            mommyControl = null;
        }

        public TControl GetControl()
        {
            return control;
        }

        public void AddControl(Control babyControl)
        {
            if (babyControl == null) {
                return;
            }
            
            GetControl().Controls.Add(babyControl);
        }

        public void RemoveControl(Control babyControl)
        {
            if (babyControl == null) {
                return;
            }
            
            GetControl().Controls.Remove(babyControl);
        }
        
        public void UpdateControl()
        {
            GetControl().Refresh();
        }
    }
}