using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleshoppingConsoleArticulo.Models
{
    public class ListaArticulos
    {


        // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class NewDataSet
        {

            private NewDataSetTable[] tableField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Table")]
            public NewDataSetTable[] Table
            {
                get
                {
                    return this.tableField;
                }
                set
                {
                    this.tableField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class NewDataSetTable
        {

            private string codigoField;

            private string descripcionField;

            private int estadoField;

            private string talleField;

            private string talleDescrField;

            private string colorField;

            private string colorDescrField;

            private string familiaField;

            private object comentariosField;

            private int stockField;

            private decimal pRECIOField;

            private decimal callBackSField;

            private decimal eSPECIALField;

            private decimal rEGALOField;

            private decimal eNTRANTEField;

            private decimal sALIENTEField;

            private decimal pRECIOUSDField;

            /// <remarks/>
            public string codigo
            {
                get
                {
                    return this.codigoField;
                }
                set
                {
                    this.codigoField = value;
                }
            }

            /// <remarks/>
            public string descripcion
            {
                get
                {
                    return this.descripcionField;
                }
                set
                {
                    this.descripcionField = value;
                }
            }

            /// <remarks/>
            public int estado
            {
                get
                {
                    return this.estadoField;
                }
                set
                {
                    this.estadoField = value;
                }
            }

            /// <remarks/>
            public string talle
            {
                get
                {
                    return this.talleField;
                }
                set
                {
                    this.talleField = value;
                }
            }

            /// <remarks/>
            public string talleDescr
            {
                get
                {
                    return this.talleDescrField;
                }
                set
                {
                    this.talleDescrField = value;
                }
            }

            /// <remarks/>
            public string color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            public string colorDescr
            {
                get
                {
                    return this.colorDescrField;
                }
                set
                {
                    this.colorDescrField = value;
                }
            }

            /// <remarks/>
            public string familia
            {
                get
                {
                    return this.familiaField;
                }
                set
                {
                    this.familiaField = value;
                }
            }

            /// <remarks/>
            public object comentarios
            {
                get
                {
                    return this.comentariosField;
                }
                set
                {
                    this.comentariosField = value;
                }
            }

            /// <remarks/>
            public int stock
            {
                get
                {
                    return this.stockField;
                }
                set
                {
                    this.stockField = value;
                }
            }

            /// <remarks/>
            public decimal PRECIO
            {
                get
                {
                    return this.pRECIOField;
                }
                set
                {
                    this.pRECIOField = value;
                }
            }

            /// <remarks/>
            public decimal CallBackS
            {
                get
                {
                    return this.callBackSField;
                }
                set
                {
                    this.callBackSField = value;
                }
            }

            /// <remarks/>
            public decimal ESPECIAL
            {
                get
                {
                    return this.eSPECIALField;
                }
                set
                {
                    this.eSPECIALField = value;
                }
            }

            /// <remarks/>
            public decimal REGALO
            {
                get
                {
                    return this.rEGALOField;
                }
                set
                {
                    this.rEGALOField = value;
                }
            }

            /// <remarks/>
            public decimal ENTRANTE
            {
                get
                {
                    return this.eNTRANTEField;
                }
                set
                {
                    this.eNTRANTEField = value;
                }
            }

            /// <remarks/>
            public decimal SALIENTE
            {
                get
                {
                    return this.sALIENTEField;
                }
                set
                {
                    this.sALIENTEField = value;
                }
            }

            public decimal PRECIOUSD
            {
                get
                {
                    return this.pRECIOUSDField;
                }
                set
                {
                    this.pRECIOUSDField = value;
                }
            }
        }




    }
}
