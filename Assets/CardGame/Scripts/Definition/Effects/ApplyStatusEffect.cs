using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class ApplyStatusEffect:IntegerEffect
    {
        private DRStatus statusTemplate;
        public ApplyStatusEffect(int value_, EffectTargetType target_, int templateId)
        {
            Value = value_;
            Target = target_;
            statusTemplate=GameEntry.DataTable.GetDataTable<DRStatus>().GetDataRow(templateId);
            if (statusTemplate==null)
            {
                Log.Error("没获取到状态");
            }
        }
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            
        }
    }
}