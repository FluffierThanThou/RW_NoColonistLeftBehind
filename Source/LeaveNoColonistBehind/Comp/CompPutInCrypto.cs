using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

using RimWorld;


namespace Fluffy
{
    public class CompAnimalCrypto : ThingComp
    {
        public new CompPropertiesAnimalCrypto props;

        public override void Initialize(CompProperties vprops)
        {
            base.Initialize(vprops);
            this.props = (vprops as CompPropertiesAnimalCrypto);
            if (this.props == null)
            {
#if DEBUG
                Log.Warning("Props went horribly wrong.");
#endif
                this.props.maxSizeAllowance = 1.5f;
            }
        }

        public override IEnumerable<Command> CompGetGizmosExtra()
        {
            if (((Building_CryptosleepCasket)this.parent).GetContainer().Count == 0)
            {

                var com = new Command_Action
                {
                    action = delegate                 {
                    List<Pawn> list = Find.ListerPawns.PawnsInFaction(Faction.OfColony).Where(p => p.RaceProps.Animal && !p.InContainer && p.BodySize < this.props.maxSizeAllowance)
                                                 .OrderBy(p => p.LabelCap).ToList();

                    if (list.Count > 0)
                    {
                        List<FloatMenuOption> list2 = new List<FloatMenuOption>();
                        list2.AddRange(list.ConvertAll<FloatMenuOption>((Pawn p) => new FloatMenuOption(p.LabelCap, delegate
                        {
#if DEBUG
                            Log.Message(p.LabelCap + " sent to " + parent.LabelCap + ", bodySize " + p.BodySize.ToString() + " sizeAllowance " + this.props.maxSizeAllowance);
#endif
                            Designation_PutInCrypto des = new Designation_PutInCrypto();
                            des.target = p;
                            des.target2 = this.parent;
                            des.def = PutInCrypto.designationDef;
                            Find.DesignationManager.RemoveAllDesignationsOn(p, false);
                            Find.DesignationManager.AddDesignation(des);
                        }, MenuOptionPriority.Medium, null, null)));
                        Find.WindowStack.Add(new FloatMenu(list2, false));
                    }
                },
                    defaultLabel = "Designate animal",
                    defaultDesc = "Designates an animal to be placed in cryptosleep.\nNo Colonist Left Behind!",
                    icon = ContentFinder<Texture2D>.Get("UI/Buttons/PutInCrypto", true)
                };

                yield return com;
            }
            yield break;
        }
    }
}

