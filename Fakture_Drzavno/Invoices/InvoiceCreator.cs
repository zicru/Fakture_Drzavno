using Fakture_Drzavno.Invoices.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Fakture_Drzavno.Invoices
{
    internal class InvoiceCreator
    {
        public static void CreateSimpleElement(ref XmlDocument document, ref XmlElement parentElement, SimpleInvoiceElement newElement)
        {
            if (String.IsNullOrEmpty(newElement.Value))
            {
                return;
            }

            XmlElement element = document.CreateElement("cbc", newElement.Name, "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            if (newElement.Attributes != null)
            {
                foreach (var attribute in newElement.Attributes)
                {
                    if (attribute.Value != null)
                    {
                        element.SetAttribute(attribute.Name, attribute.Value);
                    }
                }
            }

            XmlText value = document.CreateTextNode(newElement.Value);
            element.AppendChild(value);

            parentElement.AppendChild(element);
        }

        public static XmlElement CreateComplexElement(ref XmlDocument document, ref XmlElement parentElement, ComplexInvoiceElement newElement)
        {
            XmlElement element = document.CreateElement("cac", newElement.Name, "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            if (newElement.Attributes != null)
            {
                foreach (var attribute in newElement.Attributes)
                {
                    element.SetAttribute(attribute.Name, attribute.Value);
                }
            }

            foreach (var childElement in newElement.Elements)
            {
                if (childElement == null) continue;

                if (childElement.GetType() == typeof(SimpleInvoiceElement))
                {
                    InvoiceCreator.CreateSimpleElement(ref document, ref element, (SimpleInvoiceElement)childElement);
                }
                else if (childElement.GetType() == typeof(ComplexInvoiceElement))
                {
                    InvoiceCreator.CreateComplexElement(ref document, ref element, (ComplexInvoiceElement)childElement);
                }
            }

            if (HasValues(newElement))
            {
                parentElement.AppendChild(element);
            }

            return element;
        }

        public static void CreateComplexElement(ref XmlDocument document, ref XmlElement parentElement, ComplexInvoiceElement newElement, XMLSchema schema)
        {
            XmlElement element = document.CreateElement(schema.Name, newElement.Name, schema.URL);

            if (newElement.Attributes != null)
            {
                foreach (var attribute in newElement.Attributes)
                {
                    element.SetAttribute(attribute.Name, attribute.Value);
                }
            }

            foreach (var childElement in newElement.Elements)
            {
                if (childElement.GetType() == typeof(SimpleInvoiceElement))
                {
                    InvoiceCreator.CreateSimpleElement(ref document, ref element, (SimpleInvoiceElement)childElement);
                }
                else if (childElement.GetType() == typeof(ComplexInvoiceElement))
                {
                    InvoiceCreator.CreateComplexElement(ref document, ref element, (ComplexInvoiceElement)childElement, ((ComplexInvoiceElement)childElement).Schema);
                }
            }

            if (HasValues(newElement))
            {
                parentElement.AppendChild(element);
            }
        }

        static bool HasValues(ComplexInvoiceElement element)
        {
            var hasValues = false;

            foreach (var childElement in element.Elements)
            {
                if (childElement == null) continue;

                if (!hasValues)
                {
                    if (childElement.GetType() == typeof(SimpleInvoiceElement))
                    {
                        hasValues = !(String.IsNullOrEmpty(((SimpleInvoiceElement)childElement).Value));
                    }
                    else if (childElement.GetType() == typeof(ComplexInvoiceElement))
                    {
                        hasValues = HasValues((ComplexInvoiceElement)childElement);
                    }
                }
                else
                {
                    break;
                }
            }

            return hasValues;
        }
    }
}
