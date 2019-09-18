using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    /// <summary>
    /// Asset stream-based template cycle provider Master.
    /// </summary>
    public class AssetStreamBasedTemplateCycleProviderMaster : ITemplateCycleProviderMaster
    {
        #region Private fields

        /// <summary>
        /// Stream-based template cycle provider.
        /// </summary>
        private readonly IStreamBasedTemplateCycleProvider streamBasedTemplateCycleProvider;

        /// <summary>
        /// Context containing template cycles.
        /// </summary>
        private readonly Context context;

        /// <summary>
        /// Path to template cycles.
        /// </summary>
        private readonly string path;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates asset stream-based template cycle provider Master.
        /// </summary>
        /// <param name="streamBasedTemplateCycleProvider">Stream-based template cycle provider.
        /// Must not be null.</param>
        /// <param name="context">Context containing template cycles.
        /// Must not be null.</param>
        /// <param name="path">Path to template cycles directory. Must contain only template cycles.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="streamBasedTemplateCycleProvider"/>,
        /// <paramref name="context"/> or <paramref name="path"/> is null.</exception>
        public AssetStreamBasedTemplateCycleProviderMaster(
            IStreamBasedTemplateCycleProvider streamBasedTemplateCycleProvider,
            Context context,
            string path
            )
        {
            if (streamBasedTemplateCycleProvider == null)
                throw new ArgumentNullException(nameof(streamBasedTemplateCycleProvider));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            this.streamBasedTemplateCycleProvider = streamBasedTemplateCycleProvider;
            this.context = context;
            this.path = path;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns names of all template cycles.
        /// </summary>
        /// <returns>Names of all template cycles.</returns>
        public string[] NamesOfAllTemplateCycles()
        {
            string[] templateCycleAssets = this.context.Assets.List(path);

            string[] templateCycleNames = new string[templateCycleAssets.Length];

            for (int i = 0; i < templateCycleAssets.Length; i++)
            {
                string templateCycleAssetPath = Path.Combine(path, templateCycleAssets[i]);

                using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                {
                    (string CycleTemplateName, Lift TemplateLift) cycleTemplateNameAndLift =
                        this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLift(templateCycleStream);

                    templateCycleNames[i] = cycleTemplateNameAndLift.CycleTemplateName;
                }
            }

            return templateCycleNames;
        }

        /// <summary>
        /// Returns names of template cycles for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to return names of template cycles for.
        /// Must be specified.</param>
        /// <returns>Names of template cycles for the <see cref="Lift"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="lift"/> is unspecified.</exception>
        public string[] NamesOfTemplateCyclesForTheLift(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            string[] templateCycleAssets = this.context.Assets.List(path);

            List<string> templateCycleNames = new List<string>();

            for (int i = 0; i < templateCycleAssets.Length; i++)
            {
                string templateCycleAssetPath = Path.Combine(path, templateCycleAssets[i]);

                using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                {
                    (string CycleTemplateName, Lift TemplateLift) cycleTemplateNameAndLift =
                        this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLift(templateCycleStream);

                    if (cycleTemplateNameAndLift.TemplateLift.HasFlag(lift))
                        templateCycleNames.Add(cycleTemplateNameAndLift.CycleTemplateName);
                }
            }

            return templateCycleNames.ToArray();
        }

        /// <summary>
        /// Returns template cycle.
        /// </summary>
        /// <param name="cycleTemplateName">Name of template cycle to return. Must not be null.</param>
        /// <returns>Template cycle.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cycleTemplateName"/> is null.</exception>
        /// <exception cref="ArgumentException">No template cycle with <paramref name="cycleTemplateName"/>
        /// found.</exception>
        public TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(string cycleTemplateName)
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            string[] templateCycleAssets = this.context.Assets.List(path);

            for (int i = 0; i < templateCycleAssets.Length; i++)
            {
                string templateCycleAssetPath = Path.Combine(path, templateCycleAssets[i]);
                (string CycleTemplateName, Lift TemplateLift) cycleTemplateNameAndLift;

                using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                    cycleTemplateNameAndLift =
                        this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLift(templateCycleStream);

                if (cycleTemplateNameAndLift.CycleTemplateName == cycleTemplateName)
                    using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                        return this.streamBasedTemplateCycleProvider.TemplateCycle(templateCycleStream);
            }

            throw new ArgumentException(
                "No template cycle with specified cycle template name found.",
                nameof(cycleTemplateName)
                );
        }

        #endregion
    }
}