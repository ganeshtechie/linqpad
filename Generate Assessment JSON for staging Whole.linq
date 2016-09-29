<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	string assessmentXml = "<Wizard><Title>Mathematics</Title><Description>This assessment will have 2 questions. Each question carries 10 marks. You need to answer atleast 1 question correctly inorder to pass this assessment. Note: You cannot retake this assessment. This is your first &amp; last attempt. </Description><MaximumRetakeLimit>0</MaximumRetakeLimit><Feedback><FeedbackMethod>custom</FeedbackMethod><FeedbackMessage>Thank you for submitting the assessment</FeedbackMessage><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed the test</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Message>Well done.  You've passed the test</Message><Score>10</Score></Grade></Grades></Feedback><StagingMethod>one-by-one</StagingMethod><Scoring ScoringMethod=\"divide\" /><Page><Section><Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"checkbox\"><Title>Select all the choices which you feel could be correct. 4 + 4 = _____ ? </Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>8</Title><Value>8</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>08</Title><Value>08</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice><Choice ID=\"4\" Correct=\"false\" Score=\"0\"><Title>1</Title><Value>1</Value></Choice></Question><Question ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"radiobutton\"><Title>8 * 8 = _____ ?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>84</Title><Value>84</Value></Choice><Choice ID=\"2\" Correct=\"false\" Score=\"0\"><Title>54</Title><Value>54</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice></Question></Section></Page></Wizard>";
	
	XElement assessment = XElement.Parse(assessmentXml);
	
	var questions = assessment.Descendants("Question").Select(question => question);
	
	string stagingMethod = assessment.Element("StagingMethod").Value;
	
	
	XElement rootElement = new XElement("Staging");
	rootElement.Add(new XElement("staging_method", stagingMethod));
	
	foreach(var question in questions)
	{
		XElement element = XElement.Parse(ConvertQuestionFromXmlToJson(question.ToString()));
		element.Name = "datasource";
		rootElement.Add(element);
	}
	
	
	XmlDocument doc = new XmlDocument();
	doc.LoadXml(rootElement.ToString());

	string json = JsonConvert.SerializeXmlNode(doc);
	
	Console.WriteLine(json);
	
									
									
	
}

// Define other methods and classes here
public string ConvertQuestionFromXmlToJson(string questionXml)
  {

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
              questionElement.Add(new XElement("choice", new XElement("id", cid), new XElement("title", cText)));
          }
      }
      else if(type == "singleline" || type == "multiline")
      {

      }

      return questionElement.ToString();

  }
