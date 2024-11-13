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
using Genre = GenerokarmeleOntology.Genre;
using Person = PersonakarmeleOntology.Person;

namespace PeliculakarmeleOntology
{
	[ExcludeFromCodeCoverage]
	public class Movie : GnossOCBase
	{
		public Movie() : base() { } 

		public Movie(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_genre = new List<Genre>();
			SemanticPropertyModel propSchema_genre = pSemCmsModel.GetPropertyByPath("http://schema.org/genre");
			if(propSchema_genre != null && propSchema_genre.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_genre.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Genre schema_genre = new Genre(propValue.RelatedEntity,idiomaUsuario);
						Schema_genre.Add(schema_genre);
					}
				}
			}
			Schema_author = new List<Person>();
			SemanticPropertyModel propSchema_author = pSemCmsModel.GetPropertyByPath("http://schema.org/author");
			if(propSchema_author != null && propSchema_author.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_author.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_author = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_author.Add(schema_author);
					}
				}
			}
			Schema_rating = new List<Rating>();
			SemanticPropertyModel propSchema_rating = pSemCmsModel.GetPropertyByPath("http://schema.org/rating");
			if(propSchema_rating != null && propSchema_rating.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_rating.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Rating schema_rating = new Rating(propValue.RelatedEntity,idiomaUsuario);
						Schema_rating.Add(schema_rating);
					}
				}
			}
			Schema_director = new List<Person>();
			SemanticPropertyModel propSchema_director = pSemCmsModel.GetPropertyByPath("http://schema.org/director");
			if(propSchema_director != null && propSchema_director.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_director.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_director = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_director.Add(schema_director);
					}
				}
			}
			Schema_actor = new List<Person>();
			SemanticPropertyModel propSchema_actor = pSemCmsModel.GetPropertyByPath("http://schema.org/actor");
			if(propSchema_actor != null && propSchema_actor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_actor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_actor = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_actor.Add(schema_actor);
					}
				}
			}
			SemanticPropertyModel propSchema_url = pSemCmsModel.GetPropertyByPath("http://schema.org/url");
			this.Schema_url = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_url != null && propSchema_url.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_url.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_url.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_productionCompany = pSemCmsModel.GetPropertyByPath("http://schema.org/productionCompany");
			this.Schema_productionCompany = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_productionCompany != null && propSchema_productionCompany.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_productionCompany.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_productionCompany.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_recordedAt = pSemCmsModel.GetPropertyByPath("http://schema.org/recordedAt");
			this.Schema_recordedAt = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_recordedAt != null && propSchema_recordedAt.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_recordedAt.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_recordedAt.Add(idiomaUsuario,aux);
			}
			this.Schema_countryOfOrigin = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/countryOfOrigin"));
			SemanticPropertyModel propSchema_contentRating = pSemCmsModel.GetPropertyByPath("http://schema.org/contentRating");
			this.Schema_contentRating = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_contentRating != null && propSchema_contentRating.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_contentRating.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_contentRating.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_inLanguage = pSemCmsModel.GetPropertyByPath("http://schema.org/inLanguage");
			this.Schema_inLanguage = new List<string>();
			if (propSchema_inLanguage != null && propSchema_inLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_inLanguage.PropertyValues)
				{
					this.Schema_inLanguage.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("http://schema.org/award");
			this.Schema_award = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_award.Add(idiomaUsuario,aux);
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/description"));
			this.Schema_image = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/image"));
			this.Schema_aggregateRating = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/aggregateRating")).Value;
var item0 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/datePublished"));
if(item0.HasValue){
			this.Schema_datePublished = item0.Value;
}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/name"));
			this.Schema_duration = new Dictionary<LanguageEnum,string>();
			this.Schema_duration.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/duration")));
			
		}

		public Movie(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_genre = new List<Genre>();
			SemanticPropertyModel propSchema_genre = pSemCmsModel.GetPropertyByPath("http://schema.org/genre");
			if(propSchema_genre != null && propSchema_genre.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_genre.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Genre schema_genre = new Genre(propValue.RelatedEntity,idiomaUsuario);
						Schema_genre.Add(schema_genre);
					}
				}
			}
			Schema_author = new List<Person>();
			SemanticPropertyModel propSchema_author = pSemCmsModel.GetPropertyByPath("http://schema.org/author");
			if(propSchema_author != null && propSchema_author.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_author.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_author = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_author.Add(schema_author);
					}
				}
			}
			Schema_rating = new List<Rating>();
			SemanticPropertyModel propSchema_rating = pSemCmsModel.GetPropertyByPath("http://schema.org/rating");
			if(propSchema_rating != null && propSchema_rating.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_rating.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Rating schema_rating = new Rating(propValue.RelatedEntity,idiomaUsuario);
						Schema_rating.Add(schema_rating);
					}
				}
			}
			Schema_director = new List<Person>();
			SemanticPropertyModel propSchema_director = pSemCmsModel.GetPropertyByPath("http://schema.org/director");
			if(propSchema_director != null && propSchema_director.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_director.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_director = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_director.Add(schema_director);
					}
				}
			}
			Schema_actor = new List<Person>();
			SemanticPropertyModel propSchema_actor = pSemCmsModel.GetPropertyByPath("http://schema.org/actor");
			if(propSchema_actor != null && propSchema_actor.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_actor.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_actor = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_actor.Add(schema_actor);
					}
				}
			}
			SemanticPropertyModel propSchema_url = pSemCmsModel.GetPropertyByPath("http://schema.org/url");
			this.Schema_url = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_url != null && propSchema_url.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_url.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_url.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_productionCompany = pSemCmsModel.GetPropertyByPath("http://schema.org/productionCompany");
			this.Schema_productionCompany = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_productionCompany != null && propSchema_productionCompany.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_productionCompany.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_productionCompany.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_recordedAt = pSemCmsModel.GetPropertyByPath("http://schema.org/recordedAt");
			this.Schema_recordedAt = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_recordedAt != null && propSchema_recordedAt.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_recordedAt.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_recordedAt.Add(idiomaUsuario,aux);
			}
			this.Schema_countryOfOrigin = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/countryOfOrigin"));
			SemanticPropertyModel propSchema_contentRating = pSemCmsModel.GetPropertyByPath("http://schema.org/contentRating");
			this.Schema_contentRating = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_contentRating != null && propSchema_contentRating.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_contentRating.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_contentRating.Add(idiomaUsuario,aux);
			}
			SemanticPropertyModel propSchema_inLanguage = pSemCmsModel.GetPropertyByPath("http://schema.org/inLanguage");
			this.Schema_inLanguage = new List<string>();
			if (propSchema_inLanguage != null && propSchema_inLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_inLanguage.PropertyValues)
				{
					this.Schema_inLanguage.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("http://schema.org/award");
			this.Schema_award = new Dictionary<LanguageEnum,List<string>>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				List<string> aux = new List<string>();
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					aux.Add(propValue.Value);
				}
				this.Schema_award.Add(idiomaUsuario,aux);
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/description"));
			this.Schema_image = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/image"));
			this.Schema_aggregateRating = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/aggregateRating")).Value;
var item1 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/datePublished"));
if(item1.HasValue){
			this.Schema_datePublished = item1.Value;
}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/name"));
			this.Schema_duration = new Dictionary<LanguageEnum,string>();
			this.Schema_duration.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/duration")));
			
		}

		public virtual string RdfType { get { return "http://schema.org/Movie"; } }
		public virtual string RdfsLabel { get { return "http://schema.org/Movie"; } }
		[LABEL(LanguageEnum.es,"Género")]
		[RDFProperty("http://schema.org/genre")]
		public  List<Genre> Schema_genre { get; set;}
		public List<string> IdsSchema_genre { get; set;}

		[LABEL(LanguageEnum.es,"Guión")]
		[RDFProperty("http://schema.org/author")]
		public  List<Person> Schema_author { get; set;}
		public List<string> IdsSchema_author { get; set;}

		[LABEL(LanguageEnum.es,"Valoraciones")]
		[RDFProperty("http://schema.org/rating")]
		public  List<Rating> Schema_rating { get; set;}

		[LABEL(LanguageEnum.es,"Dirección")]
		[RDFProperty("http://schema.org/director")]
		public  List<Person> Schema_director { get; set;}
		public List<string> IdsSchema_director { get; set;}

		[LABEL(LanguageEnum.es,"Reparto")]
		[RDFProperty("http://schema.org/actor")]
		public  List<Person> Schema_actor { get; set;}
		public List<string> IdsSchema_actor { get; set;}

		[LABEL(LanguageEnum.es,"Enlace externo")]
		[RDFProperty("http://schema.org/url")]
		public  Dictionary<LanguageEnum,List<string>> Schema_url { get; set;}

		[LABEL(LanguageEnum.es,"Compañía")]
		[RDFProperty("http://schema.org/productionCompany")]
		public  Dictionary<LanguageEnum,List<string>> Schema_productionCompany { get; set;}

		[LABEL(LanguageEnum.es,"Año")]
		[RDFProperty("http://schema.org/recordedAt")]
		public  Dictionary<LanguageEnum,List<string>> Schema_recordedAt { get; set;}

		[LABEL(LanguageEnum.es,"País")]
		[RDFProperty("http://schema.org/countryOfOrigin")]
		public  string Schema_countryOfOrigin { get; set;}

		[LABEL(LanguageEnum.es,"Clasificación del contenido")]
		[RDFProperty("http://schema.org/contentRating")]
		public  Dictionary<LanguageEnum,List<string>> Schema_contentRating { get; set;}

		[LABEL(LanguageEnum.es,"Idioma")]
		[RDFProperty("http://schema.org/inLanguage")]
		public  List<string> Schema_inLanguage { get; set;}

		[LABEL(LanguageEnum.es,"Premios")]
		[RDFProperty("http://schema.org/award")]
		public  Dictionary<LanguageEnum,List<string>> Schema_award { get; set;}

		[LABEL(LanguageEnum.es,"Sinopsis")]
		[RDFProperty("http://schema.org/description")]
		public  string Schema_description { get; set;}

		[LABEL(LanguageEnum.es,"Imagen")]
		[RDFProperty("http://schema.org/image")]
		public  string Schema_image { get; set;}

		[LABEL(LanguageEnum.es,"Valoración media")]
		[RDFProperty("http://schema.org/aggregateRating")]
		public  float Schema_aggregateRating { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de publicación")]
		[RDFProperty("http://schema.org/datePublished")]
		public  DateTime Schema_datePublished { get; set;}

		[LABEL(LanguageEnum.es,"Título original")]
		[RDFProperty("http://schema.org/name")]
		public  string Schema_name { get; set;}

		[LABEL(LanguageEnum.es,"Duración")]
		[RDFProperty("http://schema.org/duration")]
		public  Dictionary<LanguageEnum,string> Schema_duration { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("schema:genre", this.IdsSchema_genre));
			propList.Add(new ListStringOntologyProperty("schema:author", this.IdsSchema_author));
			propList.Add(new ListStringOntologyProperty("schema:director", this.IdsSchema_director));
			propList.Add(new ListStringOntologyProperty("schema:actor", this.IdsSchema_actor));
			if(this.Schema_url != null)
			{
				foreach (LanguageEnum idioma in this.Schema_url.Keys)
				{
					propList.Add(new ListStringOntologyProperty("schema:url", this.Schema_url[idioma], idioma.ToString()));
				}
			}
			if(this.Schema_productionCompany != null)
			{
				foreach (LanguageEnum idioma in this.Schema_productionCompany.Keys)
				{
					propList.Add(new ListStringOntologyProperty("schema:productionCompany", this.Schema_productionCompany[idioma], idioma.ToString()));
				}
			}
			if(this.Schema_recordedAt != null)
			{
				foreach (LanguageEnum idioma in this.Schema_recordedAt.Keys)
				{
					propList.Add(new ListStringOntologyProperty("schema:recordedAt", this.Schema_recordedAt[idioma], idioma.ToString()));
				}
			}
			propList.Add(new StringOntologyProperty("schema:countryOfOrigin", this.Schema_countryOfOrigin));
			if(this.Schema_contentRating != null)
			{
				foreach (LanguageEnum idioma in this.Schema_contentRating.Keys)
				{
					propList.Add(new ListStringOntologyProperty("schema:contentRating", this.Schema_contentRating[idioma], idioma.ToString()));
				}
			}
			propList.Add(new ListStringOntologyProperty("schema:inLanguage", this.Schema_inLanguage));
			if(this.Schema_award != null)
			{
				foreach (LanguageEnum idioma in this.Schema_award.Keys)
				{
					propList.Add(new ListStringOntologyProperty("schema:award", this.Schema_award[idioma], idioma.ToString()));
				}
			}
			propList.Add(new StringOntologyProperty("schema:description", this.Schema_description));
			propList.Add(new StringOntologyProperty("schema:image", this.Schema_image));
			propList.Add(new StringOntologyProperty("schema:aggregateRating", this.Schema_aggregateRating.ToString()));
			propList.Add(new DateOntologyProperty("schema:datePublished", this.Schema_datePublished));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
			if(this.Schema_duration != null)
			{
				foreach (LanguageEnum idioma in this.Schema_duration.Keys)
				{
					propList.Add(new StringOntologyProperty("schema:duration", this.Schema_duration[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad schema:duration debe tener al menos un valor en el recurso: {resourceID}");
			}
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_rating!=null){
				foreach(Rating prop in Schema_rating){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRating = new OntologyEntity("http://schema.org/Rating", "http://schema.org/Rating", "schema:rating", prop.propList, prop.entList);
					entList.Add(entityRating);
					prop.Entity = entityRating;
				}
			}
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
			GetEntities();
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://schema.org/Movie>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://schema.org/Movie\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Schema_rating != null)
			{
			foreach(var item0 in this.Schema_rating)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://schema.org/Rating>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://schema.org/Rating\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/rating", $"<{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_ratingSource != null)
				{
							foreach (LanguageEnum idioma in item0.Schema_ratingSource.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}", "http://schema.org/ratingSource",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_ratingSource[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
				if(item0.Schema_ratingValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}",  "http://schema.org/ratingValue", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_ratingValue)}\"", list, " . ");
				}
			}
			}
				if(this.IdsSchema_genre != null)
				{
					foreach(var item2 in this.IdsSchema_genre)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/genre", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_author != null)
				{
					foreach(var item2 in this.IdsSchema_author)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/author", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_director != null)
				{
					foreach(var item2 in this.IdsSchema_director)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/director", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_actor != null)
				{
					foreach(var item2 in this.IdsSchema_actor)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/actor", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_url != null)
				{
							foreach (LanguageEnum idioma in this.Schema_url.Keys)
							{
								List<string> listaValores = this.Schema_url[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/url", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_productionCompany != null)
				{
							foreach (LanguageEnum idioma in this.Schema_productionCompany.Keys)
							{
								List<string> listaValores = this.Schema_productionCompany[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/productionCompany", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_recordedAt != null)
				{
							foreach (LanguageEnum idioma in this.Schema_recordedAt.Keys)
							{
								List<string> listaValores = this.Schema_recordedAt[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/recordedAt", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_countryOfOrigin != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}",  "http://schema.org/countryOfOrigin", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_countryOfOrigin)}\"", list, " . ");
				}
				if(this.Schema_contentRating != null)
				{
							foreach (LanguageEnum idioma in this.Schema_contentRating.Keys)
							{
								List<string> listaValores = this.Schema_contentRating[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/contentRating", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_inLanguage != null)
				{
					foreach(var item2 in this.Schema_inLanguage)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/inLanguage", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_award != null)
				{
							foreach (LanguageEnum idioma in this.Schema_award.Keys)
							{
								List<string> listaValores = this.Schema_award[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}",  "http://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_image != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/image",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_image)}\"", list, " . ");
				}
				if(this.Schema_aggregateRating != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}",  "http://schema.org/aggregateRating", $"{this.Schema_aggregateRating.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Schema_datePublished != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}",  "http://schema.org/datePublished", $"\"{this.Schema_datePublished.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}",  "http://schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
				if(this.Schema_duration != null)
				{
							foreach (LanguageEnum idioma in this.Schema_duration.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Movie_{ResourceID}_{ArticleID}", "http://schema.org/duration",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_duration[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"peliculakarmele\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://schema.org/Movie\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Schema_rating != null)
			{
			foreach(var item0 in this.Schema_rating)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/rating", $"<{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_ratingSource != null)
				{
							foreach (LanguageEnum idioma in item0.Schema_ratingSource.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}", "http://schema.org/ratingSource",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_ratingSource[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
				if(item0.Schema_ratingValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Rating_{ResourceID}_{item0.ArticleID}",  "http://schema.org/ratingValue", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_ratingValue)}\"", list, " . ");
				}
			}
			}
				if(this.IdsSchema_genre != null)
				{
					foreach(var item2 in this.IdsSchema_genre)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/genre", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_author != null)
				{
					foreach(var item2 in this.IdsSchema_author)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/author", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_director != null)
				{
					foreach(var item2 in this.IdsSchema_director)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/director", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_actor != null)
				{
					foreach(var item2 in this.IdsSchema_actor)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/actor", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_url != null)
				{
							foreach (LanguageEnum idioma in this.Schema_url.Keys)
							{
								List<string> listaValores = this.Schema_url[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/url", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_productionCompany != null)
				{
							foreach (LanguageEnum idioma in this.Schema_productionCompany.Keys)
							{
								List<string> listaValores = this.Schema_productionCompany[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/productionCompany", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_recordedAt != null)
				{
							foreach (LanguageEnum idioma in this.Schema_recordedAt.Keys)
							{
								List<string> listaValores = this.Schema_recordedAt[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/recordedAt", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_countryOfOrigin != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/countryOfOrigin", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_countryOfOrigin)}\"", list, " . ");
				}
				if(this.Schema_contentRating != null)
				{
							foreach (LanguageEnum idioma in this.Schema_contentRating.Keys)
							{
								List<string> listaValores = this.Schema_contentRating[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/contentRating", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_inLanguage != null)
				{
					foreach(var item2 in this.Schema_inLanguage)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/inLanguage", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_award != null)
				{
							foreach (LanguageEnum idioma in this.Schema_award.Keys)
							{
								List<string> listaValores = this.Schema_award[idioma];
								foreach (string valor in listaValores)
								{
									AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(valor)}\"", list, $"@{idioma} . ");
								}
							}
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_image != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/image",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_image)}\"", list, " . ");
				}
				if(this.Schema_aggregateRating != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/aggregateRating", $"{this.Schema_aggregateRating.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Schema_datePublished != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/datePublished", $"{this.Schema_datePublished.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
				if(this.Schema_duration != null)
				{
							foreach (LanguageEnum idioma in this.Schema_duration.Keys)
							{
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/duration",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_duration[idioma])}\"", list,  $"@{idioma} . ");
							}
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
			string descripcion = $"{this.Schema_description.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PeliculakarmeleOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Schema_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Schema_description;
		}




	}
}
