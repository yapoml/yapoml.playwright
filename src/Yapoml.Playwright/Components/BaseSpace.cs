using Microsoft.Playwright;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Components
{
    public abstract class BaseSpace<TParentSpace> : BaseSpace
    {
        protected TParentSpace _parentSpace;

        public BaseSpace(TParentSpace parentSpace, IPage context, ISpaceOptions spaceOptions)
            : base(context, spaceOptions)
        {
            _parentSpace = parentSpace;

        }
    }

    public abstract class BaseSpace
    {
        protected IPage _context;

        protected ISpaceOptions _spaceOptions;

        protected BaseSpace(IPage context, ISpaceOptions spaceOptions)
        {
            _context = context;
            _spaceOptions = spaceOptions;
        }
    }
}
