<Query Kind="Program" />

void Main()
{
	string questionXml = "<Question ID=\"1\" Rhetorical=\"true\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"20\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	XElement question = XElement.Parse(questionXml);
	
	// This is for choice based questions only
	
	string title = question.Element("Title").Value;
	int id = Convert.ToInt32(question.Attribute("ID").Value);
	string type = question.Attribute("ChoiceType").Value;
	bool required = Convert.ToBoolean(question.Attribute("Required").Value);
	
	var choice = question.Elements("Choice");
	
	
	XElement questionElement = new XElement("Question");
	
	questionElement.Add(new XElement("title", title));
	questionElement.Add(new XElement("id", id));
	questionElement.Add(new XElement("type", type));
	questionElement.Add(new XElement("required", required));
	
	if(type == "checkbox" || type == "radiobutton")
	{
		foreach (var c in choice)
		{
			int cid = Convert.ToInt32(c.Attribute("ID").Value);
			string cText = c.Element("Title").Value;
			questionElement.Add(new XElement("choice", new XElement("id", id), new XElement("title", cText)));
		}
	}
	
	
	Console.WriteLine(questionElement);
	
}

// Define other methods and classes here
