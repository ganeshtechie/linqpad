<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	//XElement element = XElement.Load(@"C:\github\linqpad\assessment_builder\datasource\assessment.xml");
	
	XElement question = XElement.Load(@"C:\github\linqpad\assessment_builder\datasource\factualquestion.xml");
	
	XElement targetQuestion = new XElement("question");
	
	targetQuestion.Add(new XElement("id", question.Attribute("ID").Value));
	targetQuestion.Add(new XElement("title", question.Element("Title").Value));
	targetQuestion.Add(new XElement("scoringAt", question.Attribute("ScoringAt").Value));
	targetQuestion.Add(new XElement("score", question.Attribute("Score").Value));
	targetQuestion.Add(new XElement("randomizeChoices", question.Attribute("RandomizeChoices").Value));
	targetQuestion.Add(new XElement("choiceType", question.Attribute("ChoiceType").Value));
	targetQuestion.Add(new XElement("required", question.Attribute("Required").Value));
	
	// tags need to be parsed
	targetQuestion.Add(new XElement("tags", "anxiety"));
	
	// choices need to be parsed
	
	var choices = question.Descendants("Choice");
	
	foreach(var c in choices)
	{
		XElement choice = new XElement("choices");
		choice.Add(new XElement("id", c.Attribute("ID").Value));
		choice.Add(new XElement("fieldformat", c.Element("FieldFormat").Value));
		choice.Add(new XElement("maximumlength", c.Element("MaximumLength").Value));
		targetQuestion.Add(choice);
	}
	
	XmlDocument doc = new XmlDocument();
    doc.LoadXml(targetQuestion.ToString());
			
	string questionJson = JsonConvert.SerializeXmlNode(doc);
	
	Console.WriteLine(questionJson);
	
}

// Define other methods and classes here