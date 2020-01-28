namespace Coding4Fun.ZuneDataViewer.ViewModel
{
    public class ProfileViewModelLocator
    {
        private static ProfileViewModel _pModel;

        public ProfileViewModelLocator()
        {
            CreateModel();
        }

        public static ProfileViewModel ProfileModel
        {
            get
            {
                if (_pModel == null)
                {
                    CreateModel();
                }

                return _pModel;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
                        "CA1822:MarkMembersAsStatic",
                        Justification = "This non-static member is needed for data binding purposes.")]
        public ProfileViewModel MainModel
        {
            get
            {
                return ProfileModel;
            }
        }

        public static void CreateModel()
        {
            if (_pModel == null)
            {
                _pModel = new ProfileViewModel();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}