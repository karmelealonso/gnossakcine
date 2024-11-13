using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.Helpers;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;
using Movie = PeliculakarmeleOntology.Movie;
using Occupation = OcupacionkarmeleOntology.Occupation;
using Awards = PremiokarmeleOntology.Awards;

namespace PersonakarmeleOntology
{
	[ExcludeFromCodeCoverage]
	public class Person : GnossOCBase
	{
		public Person() : base() { } 

		public Person(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_iAuthor = new List<Movie>();
			SemanticPropertyModel propSchema_iAuthor = pSemCmsModel.GetPropertyByPath("http://schema.org/iAuthor");
			if(propSchema_iAuthor != null && propSchema_iAuthor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iAuthor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iAuthor = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iAuthor.Add(schema_iAuthor);
					}
				}
			}
			Schema_hasOccupation = new List<Occupation>();
			SemanticPropertyModel propSchema_hasOccupation = pSemCmsModel.GetPropertyByPath("http://schema.org/hasOccupation");
			if(propSchema_hasOccupation != null && propSchema_hasOccupation.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_hasOccupation.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Occupation schema_hasOccupation = new Occupation(propValue.RelatedEntity,idiomaUsuario);
						Schema_hasOccupation.Add(schema_hasOccupation);
					}
				}
			}
			Schema_iDirector = new List<Movie>();
			SemanticPropertyModel propSchema_iDirector = pSemCmsModel.GetPropertyByPath("http://schema.org/iDirector");
			if(propSchema_iDirector != null && propSchema_iDirector.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iDirector.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iDirector = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iDirector.Add(schema_iDirector);
					}
				}
			}
			Schema_iActor = new List<Movie>();
			SemanticPropertyModel propSchema_iActor = pSemCmsModel.GetPropertyByPath("http://schema.org/iActor");
			if(propSchema_iActor != null && propSchema_iActor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iActor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iActor = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iActor.Add(schema_iActor);
					}
				}
			}
			Schema_awards = new List<Awards>();
			SemanticPropertyModel propSchema_awards = pSemCmsModel.GetPropertyByPath("http://schema.org/awards");
			if(propSchema_awards != null && propSchema_awards.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_awards.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Awards schema_awards = new Awards(propValue.RelatedEntity,idiomaUsuario);
						Schema_awards.Add(schema_awards);
					}
				}
			}
			this.Schema_birthDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/birthDate"));
			this.Schema_gender = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/gender"));
			this.Schema_birthPlace = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/birthPlace"));
			this.Schema_image = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/image"));
			SemanticPropertyModel propSchema_CreativeWork = pSemCmsModel.GetPropertyByPath("http://schema.org/CreativeWork");
			this.Schema_CreativeWork = new List<string>();
			if (propSchema_CreativeWork != null && propSchema_CreativeWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_CreativeWork.PropertyValues)
				{
					this.Schema_CreativeWork.Add(propValue.Value);
				}
			}
			this.Schema_startDate = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/startDate"));
			this.Schema_nationality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/nationality"));
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/name"));
		}

		public Person(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_iAuthor = new List<Movie>();
			SemanticPropertyModel propSchema_iAuthor = pSemCmsModel.GetPropertyByPath("http://schema.org/iAuthor");
			if(propSchema_iAuthor != null && propSchema_iAuthor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iAuthor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iAuthor = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iAuthor.Add(schema_iAuthor);
					}
				}
			}
			Schema_hasOccupation = new List<Occupation>();
			SemanticPropertyModel propSchema_hasOccupation = pSemCmsModel.GetPropertyByPath("http://schema.org/hasOccupation");
			if(propSchema_hasOccupation != null && propSchema_hasOccupation.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_hasOccupation.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Occupation schema_hasOccupation = new Occupation(propValue.RelatedEntity,idiomaUsuario);
						Schema_hasOccupation.Add(schema_hasOccupation);
					}
				}
			}
			Schema_iDirector = new List<Movie>();
			SemanticPropertyModel propSchema_iDirector = pSemCmsModel.GetPropertyByPath("http://schema.org/iDirector");
			if(propSchema_iDirector != null && propSchema_iDirector.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iDirector.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iDirector = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iDirector.Add(schema_iDirector);
					}
				}
			}
			Schema_iActor = new List<Movie>();
			SemanticPropertyModel propSchema_iActor = pSemCmsModel.GetPropertyByPath("http://schema.org/iActor");
			if(propSchema_iActor != null && propSchema_iActor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_iActor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Movie schema_iActor = new Movie(propValue.RelatedEntity,idiomaUsuario);
						Schema_iActor.Add(schema_iActor);
					}
				}
			}
			Schema_awards = new List<Awards>();
			SemanticPropertyModel propSchema_awards = pSemCmsModel.GetPropertyByPath("http://schema.org/awards");
			if(propSchema_awards != null && propSchema_awards.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_awards.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Awards schema_awards = new Awards(propValue.RelatedEntity,idiomaUsuario);
						Schema_awards.Add(schema_awards);
					}
				}
			}
			this.Schema_birthDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/birthDate"));
			this.Schema_gender = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/gender"));
			this.Schema_birthPlace = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/birthPlace"));
			this.Schema_image = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/image"));
			SemanticPropertyModel propSchema_CreativeWork = pSemCmsModel.GetPropertyByPath("http://schema.org/CreativeWork");
			this.Schema_CreativeWork = new List<string>();
			if (propSchema_CreativeWork != null && propSchema_CreativeWork.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_CreativeWork.PropertyValues)
				{
					this.Schema_CreativeWork.Add(propValue.Value);
				}
			}
			this.Schema_startDate = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/startDate"));
			this.Schema_nationality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/nationality"));
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/name"));
		}

		public virtual string RdfType { get { return "http://schema.org/Person"; } }
		public virtual string RdfsLabel { get { return "http://schema.org/Person"; } }
		[LABEL(LanguageEnum.es,"Ha guionizado: ")]
		[RDFProperty("http://schema.org/iAuthor")]
		public  List<Movie> Schema_iAuthor { get; set;}
		public List<string> IdsSchema_iAuthor { get; set;}

		[LABEL(LanguageEnum.es,"Ocupación")]
		[RDFProperty("http://schema.org/hasOccupation")]
		public  List<Occupation> Schema_hasOccupation { get; set;}
		public List<string> IdsSchema_hasOccupation { get; set;}

		[LABEL(LanguageEnum.es,"Ha dirigido:")]
		[RDFProperty("http://schema.org/iDirector")]
		public  List<Movie> Schema_iDirector { get; set;}
		public List<string> IdsSchema_iDirector { get; set;}

		[LABEL(LanguageEnum.es,"Ha actuado en:")]
		[RDFProperty("http://schema.org/iActor")]
		public  List<Movie> Schema_iActor { get; set;}
		public List<string> IdsSchema_iActor { get; set;}

		[LABEL(LanguageEnum.es,"Premios")]
		[RDFProperty("http://schema.org/awards")]
		public  List<Awards> Schema_awards { get; set;}
		public List<string> IdsSchema_awards { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de nacimiento")]
		[RDFProperty("http://schema.org/birthDate")]
		public  DateTime? Schema_birthDate { get; set;}

		[LABEL(LanguageEnum.es,"Sexo o género")]
		[RDFProperty("http://schema.org/gender")]
		public  string Schema_gender { get; set;}

		[LABEL(LanguageEnum.es,"Lugar de nacimiento")]
		[RDFProperty("http://schema.org/birthPlace")]
		public  string Schema_birthPlace { get; set;}

		[LABEL(LanguageEnum.es,"")]
		[RDFProperty("http://schema.org/image")]
		public  string Schema_image { get; set;}

		[LABEL(LanguageEnum.es,"Obras notables")]
		[RDFProperty("http://schema.org/CreativeWork")]
		public  List<string> Schema_CreativeWork { get; set;}

		[LABEL(LanguageEnum.es,"Año de nacimiento")]
		[RDFProperty("http://schema.org/startDate")]
		public  int? Schema_startDate { get; set;}

		[LABEL(LanguageEnum.es,"País de ciudadanía")]
		[RDFProperty("http://schema.org/nationality")]
		public  string Schema_nationality { get; set;}

		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("http://schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("schema:iAuthor", this.IdsSchema_iAuthor));
			propList.Add(new ListStringOntologyProperty("schema:hasOccupation", this.IdsSchema_hasOccupation));
			propList.Add(new ListStringOntologyProperty("schema:iDirector", this.IdsSchema_iDirector));
			propList.Add(new ListStringOntologyProperty("schema:iActor", this.IdsSchema_iActor));
			propList.Add(new ListStringOntologyProperty("schema:awards", this.IdsSchema_awards));
			if (this.Schema_birthDate.HasValue){
				propList.Add(new DateOntologyProperty("schema:birthDate", this.Schema_birthDate.Value));
				}
			propList.Add(new StringOntologyProperty("schema:gender", this.Schema_gender));
			propList.Add(new StringOntologyProperty("schema:birthPlace", this.Schema_birthPlace));
			propList.Add(new StringOntologyProperty("schema:image", this.Schema_image));
			propList.Add(new ListStringOntologyProperty("schema:CreativeWork", this.Schema_CreativeWork));
			propList.Add(new StringOntologyProperty("schema:startDate", this.Schema_startDate.ToString()));
			propList.Add(new StringOntologyProperty("schema:nationality", this.Schema_nationality));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI)
		{
			return ToGnossApiResource(resourceAPI, new List<string>());
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<Guid> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, null, Guid.Empty, Guid.Empty, listaDeCategorias);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo, List<Guid> listaIdDeCategorias = null)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology = null;
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories = listaDeCategorias;
			resource.CategoriesIds = listaIdDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://schema.org/Person>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://schema.org/Person\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsSchema_iAuthor != null)
				{
					foreach(var item2 in this.IdsSchema_iAuthor)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/iAuthor", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_hasOccupation != null)
				{
					foreach(var item2 in this.IdsSchema_hasOccupation)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/hasOccupation", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_iDirector != null)
				{
					foreach(var item2 in this.IdsSchema_iDirector)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/iDirector", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_iActor != null)
				{
					foreach(var item2 in this.IdsSchema_iActor)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/iActor", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_awards != null)
				{
					foreach(var item2 in this.IdsSchema_awards)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/awards", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_birthDate != null && this.Schema_birthDate != DateTime.MinValue)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/birthDate", $"\"{this.Schema_birthDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_gender != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/gender", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_gender)}\"", list, " . ");
				}
				if(this.Schema_birthPlace != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/birthPlace", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace)}\"", list, " . ");
				}
				if(this.Schema_image != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/image", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_image)}\"", list, " . ");
				}
				if(this.Schema_CreativeWork != null)
				{
					foreach(var item2 in this.Schema_CreativeWork)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/CreativeWork", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_startDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/startDate", $"{this.Schema_startDate.Value.ToString()}", list, " . ");
				}
				if(this.Schema_nationality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/nationality", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_nationality)}\"", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"personakarmele\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://schema.org/Person\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdsSchema_iAuthor != null)
				{
					foreach(var item2 in this.IdsSchema_iAuthor)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/iAuthor", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_hasOccupation != null)
				{
					foreach(var item2 in this.IdsSchema_hasOccupation)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/hasOccupation", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_iDirector != null)
				{
					foreach(var item2 in this.IdsSchema_iDirector)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/iDirector", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_iActor != null)
				{
					foreach(var item2 in this.IdsSchema_iActor)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/iActor", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_awards != null)
				{
					foreach(var item2 in this.IdsSchema_awards)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/awards", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_birthDate != null && this.Schema_birthDate != DateTime.MinValue)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/birthDate", $"{this.Schema_birthDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_gender != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/gender", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_gender)}\"", list, " . ");
				}
				if(this.Schema_birthPlace != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/birthPlace", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace)}\"", list, " . ");
				}
				if(this.Schema_image != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/image", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_image)}\"", list, " . ");
				}
				if(this.Schema_CreativeWork != null)
				{
					foreach(var item2 in this.Schema_CreativeWork)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/CreativeWork", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_startDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/startDate", $"{this.Schema_startDate.Value.ToString()}", list, " . ");
				}
				if(this.Schema_nationality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/nationality", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_nationality)}\"", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PersonakarmeleOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Schema_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Schema_name;
		}




	}
}
