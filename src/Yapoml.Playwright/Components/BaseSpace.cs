using Microsoft.Playwright;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Components
{
    public abstract class BaseSpace<TParentSpace> : BaseSpace
    {
        protected TParentSpace _parentSpace;

        public BaseSpace(TParentSpace parentSpace, IPage driver, ISpaceOptions spaceOptions)
            : base(driver, spaceOptions)
        {
            _parentSpace = parentSpace;

        }
    }

    public abstract class BaseSpace
    {
        protected IPage _driver;

        protected ISpaceOptions _spaceOptions;

        protected BaseSpace(IPage driver, ISpaceOptions spaceOptions)
        {
            _driver = driver;
            _spaceOptions = spaceOptions;
        }
    }
}
