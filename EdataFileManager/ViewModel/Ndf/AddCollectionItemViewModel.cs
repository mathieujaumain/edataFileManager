using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel.Ndf
{
    public class AddCollectionItemViewModel : ViewModelBase
    {
        private NdfType _type = NdfType.Unset;
        private CollectionItemValueHolder _wrapper;
        private readonly List<NdfType> _typeSelection = new List<NdfType>();

        public ICommand OkCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
        public NdfbinManager Manager { get; protected set; }

        protected Window View { get; set; }

        public NdfType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                GetValueForType();
                OnPropertyChanged(() => Type);
            }
        }

        private void GetValueForType()
        {
            Wrapper =
                new CollectionItemValueHolder(NdfTypeManager.GetValue(new byte[NdfTypeManager.SizeofType(Type)], Type, Manager, 0), Manager, 0);
        }

        public List<NdfType> TypeSelection
        {
            get { return _typeSelection; }
        }

        public CollectionItemValueHolder Wrapper
        {
            get { return _wrapper; }
            set { _wrapper = value; OnPropertyChanged(() => Wrapper); }
        }

        public AddCollectionItemViewModel(NdfbinManager mgr, Window view)
        {
            Manager = mgr;
            View = view;

            OkCommand = new ActionCommand(OkCommandExecute, () => Type != NdfType.Unset);
            CancelCommand = new ActionCommand(CancelCommandExecute);

            SetTypeSelection();
        }

        private void CancelCommandExecute(object obj)
        {
            View.DialogResult = false;
        }

        private void OkCommandExecute(object obj)
        {
            View.DialogResult = true;
        }

        private void SetTypeSelection()
        {
            _typeSelection.AddRange(new NdfType[]
                                        {
                                            NdfType.Boolean,
                                            NdfType.Int32,
                                            NdfType.UInt32,
                                            NdfType.Float32,
                                            NdfType.ObjectReference,
                                            NdfType.Map,
                                            NdfType.List,
                                            NdfType.TableString,
                                            NdfType.TableStringFile,
                                            NdfType.LocalisationHash,
                                            NdfType.WideString,
                                            NdfType.TransTableReference,
                                            NdfType.Guid,
                                            NdfType.Int8,
                                            NdfType.Int16,
                                            NdfType.Color32,
                                            NdfType.Float64,
                                            NdfType.Float64_2,
                                            NdfType.Vector,
                                            NdfType.Color128,
                                            NdfType.MapList,
                                        });
        }
    }
}
