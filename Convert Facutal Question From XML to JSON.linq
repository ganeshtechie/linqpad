<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	string assessmentXml = "<Wizard><Title>Mathematics</Title><Description>This assessment will have 2 questions. Each question carries 10 marks. You need to answer atleast 1 question correctly inorder to pass this assessment. Note: You cannot retake this assessment. This is your first &amp; last attempt. </Description><MaximumRetakeLimit>0</MaximumRetakeLimit><Feedback><FeedbackMethod>custom</FeedbackMethod><FeedbackMessage>Thank you for submitting the assessment</FeedbackMessage><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed the test</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Message>Well done.  You've passed the test</Message><Score>10</Score></Grade></Grades></Feedback><StagingMethod>one-by-one</StagingMethod><Scoring ScoringMethod=\"divide\" /><Page><Section><Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"checkbox\"><Title>Select all the choices which you feel could be correct. 4 + 4 = _____ ? </Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>8</Title><Value>8</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>08</Title><Value>08</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice><Choice ID=\"4\" Correct=\"false\" Score=\"0\"><Title>1</Title><Value>1</Value></Choice></Question><Question ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"radiobutton\"><Title>8 * 8 = _____ ?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>84</Title><Value>84</Value></Choice><Choice ID=\"2\" Correct=\"false\" Score=\"0\"><Title>54</Title><Value>54</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice></Question></Section></Page></Wizard>";
	
	string factualQuestionXml = "<Question ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"singleline\"><Title>8 * 8 = _____ ?</Title><Choice ID=\"1\" Correct=\"true\"><FieldFormat>Email</FieldFormat><MaximumLength>2000</MaximumLength></Choice></Question>";
	
	
	XElement element = XElement.Parse(assessmentXml);
	
	//XElement question = element.Descendants("Question").Select(x => x).FirstOrDefault();
	
	XElement question = XElement.Parse(factualQuestionXml);
	
	
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
