using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.Mapper.Attributes;
using Rop.Types;

namespace Rop.Mapper
{
    /// <summary>
    /// Some complex rules are traslation of simple ones when Src and Dst are known
    /// Usualy for If and From attributes to normal ones
    /// </summary>
    internal partial class RulesCollection
    {
        private void TraslateIf(Properties srcprop, Properties dstprop)
        {
            foreach (var property in srcprop.GetAll())
            {
                var atts = property.Attributes.OfType<IMapsIfAttribute>().Where(a=>a.Dst==dstprop.ClassType).ToList();
                foreach (IMapsIfAttribute att in atts.ToList())
                {
                    switch (att)
                    {
                        case MapsIgnoreIfAttribute mignore:
                            TraslateAtt(mignore, property, dstprop);
                            break;
                        case MapsToIfAttribute mmapsto:
                            TraslateAtt(mmapsto, property, dstprop);
                            break;
                        case MapsConversorIfAttribute mconversor:
                            TraslateAtt(mconversor, property, dstprop);
                            break;
                        case MapsDateToIfAttribute mdateto:
                            TraslateAtt(mdateto, property, dstprop);
                            break;
                        case MapsTimeToIfAttribute mtimeto:
                            TraslateAtt(mtimeto, property, dstprop);
                            break;
                        case MapsFlatIfAttribute mflat:
                            TraslateAtt(mflat,property,dstprop);
                            break;
                        default:
                            throw new ArgumentException("{att} unknown");
                    }
                }
            }
        }
        private void TraslateFrom(Properties srcprop, Properties dstprop)
        {
            foreach (var property in dstprop.GetAll())
            {
                var attsfrom = property.Attributes.OfType<MapsFromAttribute>().ToList();
                foreach (var att in attsfrom)
                {
                    TraslateAtt(att,property,srcprop);
                }
                var attsfromif = property.Attributes.OfType<MapsFromIfAttribute>().Where(a=>a.Src==srcprop.ClassType).ToList();
                foreach (var att in attsfromif)
                {
                    TraslateAtt(att, property, srcprop);
                }
                var attsfromconversor = property.Attributes.OfType<MapsFromConversorAttribute>().Where(a=>a.Src==srcprop.ClassType).ToList();
                foreach (var att in attsfromconversor)
                {
                    TraslateAtt(att, property, srcprop);
                }
            }
        }
        
        private void TraslateAtt(MapsIgnoreIfAttribute ia, Property property, Properties dstprop)
        {
            property.Remove(ia);
            RemoveAtt<MapsToAttribute>(property);
            if (ia.Dst!=dstprop.ClassType) return;
            property.Add(new MapsIgnoreAttribute());
        }
        private void TraslateAtt(MapsTimeToIfAttribute ia, Property property, Properties dstprop)
        {
            property.Remove(ia);
            RemoveAtt<MapsTimeToAttribute>(property);
            if (ia.Dst!=dstprop.ClassType) return;
            property.Add(new MapsTimeToAttribute(ia.Name));
        }
        private void TraslateAtt(MapsFlatIfAttribute ia, Property property, Properties dstprop)
        {
            property.Remove(ia);
            RemoveAtt<MapsFlatIfAttribute>(property);
            if (ia.Dst!=dstprop.ClassType) return;
            property.Add(new MapsFlatAttribute());
        }
        private void TraslateAtt(MapsDateToIfAttribute ia, Property property, Properties dstprop)
        {
            property.Remove(ia);
            RemoveAtt<MapsDateToAttribute>(property);
            if (ia.Dst!=dstprop.ClassType) return;
            property.Add(new MapsDateToAttribute(ia.Name));
        }

        private void TraslateAtt(MapsToIfAttribute ia, Property property, Properties dstprop)
        {
            property.Remove(ia);
            RemoveAtt<MapsToAttribute>(property);
            property.Add(new MapsToExtraAttribute(ia.Name,ia.Conversor));
        }
        private void TraslateAtt(MapsConversorIfAttribute ia, Property property, Properties dstprop)
        {
            RemoveAtt<MapsConversorAttribute>(property);
            property.Add(new MapsConversorAttribute(ia.Conversor));
        }
        private void TraslateAtt(MapsFromAttribute ia, Property property, Properties srcprop)
        {
            var fromprop = srcprop.Get(ia.Name);
            if (fromprop is null)
            {
                property.Add(new MapsErrorAttribute(ia,property));
                return;
            }
            RemoveAtt<MapsToAttribute>(fromprop);
            fromprop.Add(new MapsToAttribute(property.PropertyName));
        }
        private void TraslateAtt(MapsFromIfAttribute ia, Property property, Properties srcprop)
        {
            var fromprop = srcprop.Get(ia.Name);
            if (fromprop is null)
            {
                property.Add(new MapsErrorAttribute(ia, property));
                return;
            }
            RemoveAtt<MapsToAttribute>(fromprop);
            fromprop.Add(new MapsToExtraAttribute(property.PropertyName,ia.Conversor));
        }

        private void TraslateAtt(MapsFromConversorAttribute ia, Property property, Properties srcprop)
        {
            var fromprop = srcprop.GetTo(property.PropertyName);
            if (fromprop is null) return;
            RemoveAtt<MapsConversorAttribute>(fromprop);
            fromprop.Add(new MapsConversorAttribute(ia.Conversor));
        }

        private void TraslateAtt(MapsIgnoreSomeAttribute ia, Properties srcprop)
        {
            var props = ia.Properties;
            TraslateIgnoreAtt(props, srcprop);
        }

        private void TraslateAtt(MapsIgnoreSomeIfAttribute ia, Properties srcprop, Type dst)
        {
            if (ia.Dst != dst) return;
            var props = ia.Properties;
            TraslateIgnoreAtt(props, srcprop);
        }

        private void TraslateIgnoreAtt(string[] name, Properties srcprop)
        {
            foreach (var s in name)
            {
                var fromprop = srcprop.GetTo(s);
                if (fromprop is null) continue;
                RemoveAtt<MapsToAttribute>(fromprop);
                fromprop.Add(new MapsIgnoreAttribute());
            }
        }



        private static void RemoveAtt<T>(Property property) where T:IMapsAttribute
        {
            while (property.HasAtt<T>(out var ma)) property.Remove(ma!);
        }

        private void AdjustProperty(Properties props)
        {
            foreach (var property in props.GetAll())
            {
                property.AdjustType();
            }
        }
    }
}
