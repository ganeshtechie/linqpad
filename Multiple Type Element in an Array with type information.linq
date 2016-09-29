<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Collections.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

// Deserialize JSON to C# Objects

void Main()
{
	string json = "{\"assessment\":{\"title\":\"Health Assessment\",\"description\":\"A Basic health assessment\",\"questions\":[{\"id\":1,\"required\":true,\"randomizeChoice\":true,\"title\":\"1) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"checkbox\",\"scoringAt\":\"question\",\"score\":1,\"choice\":[{\"correct\":false,\"title\":\"Untitled Choice - 1\",\"id\":1},{\"correct\":false,\"title\":\"Untitled Choice - 2\",\"id\":2}],\"tags\":[\"anxiety\",\"depression\"],\"edit\":false},{\"id\":2,\"required\":true,\"randomizeChoice\":true,\"title\":\"2) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"radiobutton\",\"scoringAt\":\"question\",\"score\":1,\"choice\":[{\"correct\":false,\"title\":\"Untitled Choice - 1\",\"id\":1},{\"correct\":false,\"title\":\"Untitled Choice - 2\",\"id\":2},{\"correct\":false,\"title\":\"Untitled Choice - 3\",\"id\":3}],\"tags\":[\"anxiety\"]},{\"id\":3,\"required\":true,\"randomizeChoice\":true,\"title\":\"2) Checks if predicate returns truthy for all elements of collection. Iteration is stopped once predicate returns falsey.\",\"choiceType\":\"singleline\",\"scoringAt\":\"question\",\"score\":1,\"choice\":{\"id\":1,\"fieldformat\":\"number\",\"maximumlength\":100},\"tags\":[\"anxiety\"]}],\"retake\":0,\"scoringMethod\":\"all\",\"feedbackMessage\":\"Thank you for participating\"}}";
	
	XmlDocument xml = JsonConvert.DeserializeXmlNode(json);
	
	var stringWriter = new StringWriter();
	
	using (var xmlTextWriter = XmlWriter.Create(stringWriter))
	{
		xml.WriteTo(xmlTextWriter);
		xmlTextWriter.Flush();
	}
	
	XElement element = XElement.Parse(stringWriter.GetStringBuilder().ToString());
	
	List<XElement> multipleChoices = element.Descendants("questions")
                .Where(x => x.Element("choiceType").Value == "radiobutton" || x.Element("choiceType").Value == "checkbox").ToList();
				
	List<XElement> factual = element.Descendants("questions")
                .Where(x => x.Element("choiceType").Value == "singleline" || x.Element("choiceType").Value == "multiline").ToList();
	
	
	XmlSerializer serializer = new XmlSerializer(typeof(MultipleChoiceQuestion));
	
	List<BaseQuestion> questions = new List<BaseQuestion>();
	
	foreach(XElement ele in multipleChoices)
	{
		using (TextReader reader = new StringReader(ele.ToString()))
		{
			MultipleChoiceQuestion question = ( MultipleChoiceQuestion )  serializer.Deserialize(reader);
			questions.Add(question);
		}
	}
	
	serializer = new XmlSerializer(typeof(FactualQuestion));
	
	foreach(XElement ele in factual)
	{
		using (TextReader reader = new StringReader(ele.ToString()))
		{
			FactualQuestion question = ( FactualQuestion )  serializer.Deserialize(reader);
			questions.Add(question);
		}
	}
	
	serializer = new XmlSerializer(typeof(Assessment));
	
	Assessment assessment;
	
	using (TextReader reader = new StringReader(stringWriter.GetStringBuilder().ToString()))
	{
		assessment = ( Assessment )  serializer.Deserialize(reader);
		
		assessment.questions = questions;
	}
	
	serializer = new XmlSerializer(typeof(Assessment));
   	StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\assessment.xml");
   	serializer.Serialize(sw, assessment);
   	sw.Close();
}


[XmlRoot("assessment")]
public class Assessment
{
	[XmlElement("title")]
	public string title { get; set; }
	[XmlElement("description")]
	public string description { get; set; }
	[XmlElement("question")]
	public List<BaseQuestion> questions { get; set; }
	[XmlElement("retake")]
	public int retake { get; set; }
	[XmlElement("scoringMethod")]
	public string scoringMethod { get; set; }
	[XmlElement("feedbackMessage")]
	public string feedbackMessage { get; set; }
}

[XmlInclude(typeof(MultipleChoice))]
[XmlInclude(typeof(FactualChoice))]
public abstract class BaseChoice
{
	public int id { get; set; }
}


public class MultipleChoice: BaseChoice
{
	public bool correct { get; set; }
	public string title { get; set; }
}


public class FactualChoice: BaseChoice
{
	public string fieldformat { get; set; }
	public string maximumlength { get; set; }
}

// Define other methods and classes here

[XmlInclude(typeof(MultipleChoiceQuestion))]
[XmlInclude(typeof(FactualQuestion))]
public abstract class BaseQuestion
{
	public int id { get; set; }
	public bool required { get; set; }
	public bool randomizeChoice { get; set; }
	public string title { get; set; }
	public string choiceType { get; set; }
	public string scoringAt { get; set; }
	public int score { get; set; }
	public string[] tags { get; set; }
	public string rhetorical { get; set; }
}

[XmlRoot("questions")]
public class MultipleChoiceQuestion : BaseQuestion
{
	[XmlElement("choice")]
	public List<MultipleChoice> choice { get; set; }
}

[XmlRoot("questions")]
public class FactualQuestion : BaseQuestion
{
	[XmlElement("choice")]
	public FactualChoice choices { get; set; }
}