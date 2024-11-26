using Hwdtech;
using Scriban;
using Scriban.Parsing;

namespace SpaceBattle.Lib;

public class AdaptersGenerator
{
    private Template _adapterTemplate;

    public AdaptersGenerator()
    {
        _adapterTemplate = Template.Parse(@"
        public class {{ type }}Adapter : {{ type }}
        {
            private IUObject _obj;
            public {{ type }}Adapter(IUObject obj) => _obj = obj;
            {{~ for property in (properties) ~}}
            public {{property.property_type.name}} {{property.name}}
            {
            {{~ if property.can_read ~}}
                get
                {
                    return IoC.Resolve<Vector>(""Game.IUObject.GetProperty"", _obj, ""{{property.name}}"");
                }
            {{~ end ~}}
            {{~ if property.can_write ~}}
                set
                {
                    IoC.Resolve<ICommand>(""Game.IUObject.SetProperty"", _obj, ""{{property.name}}"", value).Execute();
                }
            {{~ end ~}}
            }
        {{~ end ~}}
        }
        ");
    }

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Adapters.Generate",
        (object[] args) =>
        {
            var interfaceType = (Type)args[0];
            var interfaceProps = interfaceType.GetProperties();
            var adapterString = _adapterTemplate.Render(new
            {
                type = interfaceType.Name,
                properties = interfaceProps,
            });

            return adapterString;
        }).Execute();
    }
}

