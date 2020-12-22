// Adapted from extensions for ASP.NET Core
// https://github.com/aspnet/AspNetKatana/issues/147#issuecomment-350041434

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin
{
    using Predicate = Func<IOwinContext, bool>;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class UseWhenExtensions
    {
        /// <summary>
        /// Conditionally creates a branch in the request pipeline that is rejoined to the main pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="predicate">Invoked with the request environment to determine if the branch should be taken</param>
        /// <param name="configuration">Configures a branch to take</param>
        /// <returns></returns>
        public static IAppBuilder UseWhen(this IAppBuilder app, Predicate predicate, Action<IAppBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Create and configure the branch builder right away; otherwise,
            // we would end up running our branch after all the components
            // that were subsequently added to the main builder.
            var branchBuilder = app.New();
            configuration(branchBuilder);

            return app.Use(new Func<AppFunc, AppFunc>(main =>
            {
                // This is called only when the main application builder 
                // is built, not per request.
                branchBuilder.Run(context => main(context.Environment));
                var branch = (AppFunc)branchBuilder.Build(typeof(AppFunc));

                return context =>
                {
                    if (predicate(new OwinContext(context)))
                    {
                        return branch(context);
                    }
                    else
                    {
                        return main(context);
                    }
                };
            }));
        }
    }
}