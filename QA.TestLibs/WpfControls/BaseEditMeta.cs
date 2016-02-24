namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using XmlDesiarilization;

    public abstract class BaseEditMeta
    {
        public Button SaveButton { get; set; }
        public Button CancelButton { get; set; }
        public Button EditButton { get; set; }

        public Func<object> ControlGetValue { get; set; }
        public Action<object> ControlSetValue { get; set; }

        public object SavedValue { get; set; }
        public object CreatedValue { get; set; } = null;

        public XmlProperty ParentProperty { get; set; }
        public object ParentObject { get; set; }

        public BaseEditMeta(Button editButton)
        {
            EditButton = editButton;
            EditButton.Click += EditValueButton_Click;
        }

        protected abstract void SaveEditValueButton_Click(object sender, RoutedEventArgs e);
        protected abstract void CancelEditValueButton_Click(object sender, RoutedEventArgs e);
        protected abstract void EditValueButton_Click(object sender, RoutedEventArgs e);
    }
}
