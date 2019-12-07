using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    public class AssetStreamBasedTemplateCycleProviderMaster : ITemplateCycleProviderMaster
    {
        #region Private fields

        private readonly IStreamBasedTemplateCycleProvider streamBasedTemplateCycleProvider;
        private readonly Context context;
        private readonly string path;

        #endregion

        #region Constructors

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

        public async Task<IReadOnlyList<CycleTemplateName>> NamesOfAllTemplateCyclesAsync()
        {
            string[] templateCycleAssets = this.context.Assets.List(path);

            CycleTemplateName[] templateCycleNames = new CycleTemplateName[templateCycleAssets.Length];

            for (int i = 0; i < templateCycleAssets.Length; i++)
            {
                string templateCycleAssetPath = Path.Combine(path, templateCycleAssets[i]);

                using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                {
                    (CycleTemplateName CycleTemplateName, Lift TemplateLift) cycleTemplateNameAndLift =
                        await this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLiftAsync(templateCycleStream)
                        .ConfigureAwait(false);

                    templateCycleNames[i] = cycleTemplateNameAndLift.CycleTemplateName;
                }
            }

            return templateCycleNames;
        }

        public async Task<IReadOnlyList<CycleTemplateName>> NamesOfTemplateCyclesForTheLiftAsync(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(lift));

            string[] templateCycleAssets = this.context.Assets.List(path);

            List<CycleTemplateName> templateCycleNames = new List<CycleTemplateName>();

            for (int i = 0; i < templateCycleAssets.Length; i++)
            {
                string templateCycleAssetPath = Path.Combine(path, templateCycleAssets[i]);

                using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                {
                    (CycleTemplateName CycleTemplateName, Lift TemplateLift) cycleTemplateNameAndLift =
                        await this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLiftAsync(templateCycleStream)
                        .ConfigureAwait(false);

                    if (cycleTemplateNameAndLift.TemplateLift.HasFlag(lift))
                        templateCycleNames.Add(cycleTemplateNameAndLift.CycleTemplateName);
                }
            }

            return templateCycleNames;
        }

        public async Task<TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>> TemplateCycleAsync(
            CycleTemplateName cycleTemplateName
            )
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
                        await this.streamBasedTemplateCycleProvider.CycleTemplateNameAndLiftAsync(templateCycleStream)
                        .ConfigureAwait(false);

                if (cycleTemplateNameAndLift.CycleTemplateName == cycleTemplateName)
                    using (Stream templateCycleStream = this.context.Assets.Open(templateCycleAssetPath))
                        return await this.streamBasedTemplateCycleProvider.TemplateCycleAsync(templateCycleStream)
                            .ConfigureAwait(false);
            }

            throw new ArgumentException(
                "No template cycle with specified cycle template name found.",
                nameof(cycleTemplateName)
                );
        }

        #endregion
    }
}