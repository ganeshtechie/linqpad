<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	string responseJson = "{responses:{\"response\":[{\"questionId\":1,\"selectedChoices\":[1,2]},{\"questionId\":2,\"selectedChoices\":[1]},{\"questionId\":3,\"text\":\"<h1>test</h1>\"}]}}";
	
	XmlDocument xml = JsonConvert.DeserializeXmlNode(responseJson);
	
	var stringWriter = new StringWriter();

	using (var xmlTextWriter = XmlWriter.Create(stringWriter))
	{
		xml.WriteTo(xmlTextWriter);
		xmlTextWriter.Flush();
	}
	
	XElement element = XElement.Parse(stringWriter.GetStringBuilder().ToString());
	
	var responses = element.Descendants("response");
	
	XElement root = new XElement("Response");
	
	foreach(var r in responses)
	{
		int questionId = Convert.ToInt32(r.Element("questionId").Value);
		
		XElement question = new XElement("Question", new XAttribute("ID", questionId));
		
		var choices = r.Descendants("selectedChoices");
		
		if(r.Element("text") != null)
		{
			string response = r.Element("text").Value;
			XElement responseText = new XElement("ResponseText");
			responseText.Add(new XCData(response));
			question.Add(responseText);
		}
		
		if(choices.Count() > 0)
		{
			XElement selectedChoices = new XElement("SelectedChoices");
			foreach(var v in choices)
			{
				selectedChoices.Add(new XElement("Choice", new XElement("ID", v.Value)));
			}
			question.Add(selectedChoices);
		}
		root.Add(question);
	}
	
	Console.WriteLine(root);
	
}

// Define other methods and classes here
