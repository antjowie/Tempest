using System;
using System.Collections.Generic;
using System.Text;

namespace Tempest.Systems
{
    /**
     * Systems allow us to add logic to the project without having to manually create and maintain them.
     * Systems are created and maintained by the plugin. From there you can hook into events such as Update.
     * This allows us to only pay for what we use.
     */
    public abstract class BaseSystem
    {
        public virtual bool StripFromRelease { get; } = false;
        public abstract void Init(Tempest tempest);
    }
}
