using Microsoft.Playwright;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Components
{
    public abstract class BaseSpace<TParentSpace> : BaseSpace
    {
        protected TParentSpace _parentSpace;

        public BaseSpace(TParentSpace parentSpace, IPage webDriver, ISpaceOptions spaceOptions)
            : base(webDriver, spaceOptions)
        {
            _parentSpace = parentSpace;

        }
    }

    public abstract class BaseSpace
    {
        protected IPage _webDriver;

        protected ISpaceOptions _spaceOptions;

        protected BaseSpace(IPage webDriver, ISpaceOptions spaceOptions)
        {
            _webDriver = webDriver;
            _spaceOptions = spaceOptions;
        }
    }
}
