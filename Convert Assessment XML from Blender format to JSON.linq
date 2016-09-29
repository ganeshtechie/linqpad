<Query Kind="Program">
  <Reference>C:\tfs\BlenderFamily\CommonBin\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	string assessmentXml = "<Wizard><Title>Mathematics</Title><Description>This assessment will have 2 questions. Each question carries 10 marks. You need to answer atleast 1 question correctly inorder to pass this assessment. Note: You cannot retake this assessment. This is your first &amp; last attempt. </Description><MaximumRetakeLimit>0</MaximumRetakeLimit><Feedback><FeedbackMethod>custom</FeedbackMethod><FeedbackMessage>Thank you for submitting the assessment</FeedbackMessage><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed the test</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Message>Well done.  You've passed the test</Message><Score>10</Score></Grade></Grades></Feedback><StagingMethod>one-by-one</StagingMethod><Scoring ScoringMethod=\"divide\" /><Page><Section><Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"checkbox\"><Title>Select all the choices which you feel could be correct. 4 + 4 = _____ ? </Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>8</Title><Value>8</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>08</Title><Value>08</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice><Choice ID=\"4\" Correct=\"false\" Score=\"0\"><Title>1</Title><Value>1</Value></Choice></Question><Question ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"radiobutton\"><Title>8 * 8 = _____ ?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>84</Title><Value>84</Value></Choice><Choice ID=\"2\" Correct=\"false\" Score=\"0\"><Title>54</Title><Value>54</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>16</Title><Value>16</Value></Choice></Question></Section></Page></Wizard>";
	
	XElement element = XElement.Parse(assessmentXml);
	
	XElement assessment = new XElement("Assessment");
	
	assessment.Add(new XElement("title", element.Element("Title").Value));
	
	assessment.Add(new XElement("description", element.Element("Description").Value));
	
	assessment.Add(new XElement("retake", element.Element("MaximumRetakeLimit").Value));
	
	assessment.Add(new XElement("stagingMethod", element.Element("StagingMethod").Value));
	
	if(element.Element("Scoring") != null)
		assessment.Add(new XElement("scoring_method", element.Element("Scoring").Attribute("ScoringMethod").Value));
	
	
	var questions = element.Descendants("Question").Select(x => x);
	
	foreach(var q in questions)
	{
		assessment.Add(new XElement("questions", "Test 1"));
	}
	
	
	// Feedback
	XElement feedbackElement = new XElement("feedback");
	
	feedbackElement.Add(new XElement("feedbackMethod", element.Element("Feedback").Element("FeedbackMethod").Value));
	feedbackElement.Add(new XElement("feedbackMessage", element.Element("Feedback").Element("FeedbackMessage").Value));
	
	// Grades
	XElement elementGrade = element.Element("Feedback").Element("Grades");
	
	if(elementGrade != null)
	{
		var grades = elementGrade.Elements("Grade");
		foreach(var grade in grades)
		{
			feedbackElement.Add(new XElement("grades", 
				new XElement("name", grade.Element("Name").Value),
				new XElement("default", grade.Element("IsDefault").Value),
				new XElement("feedback", grade.Element("Message").Value)));
				
				if(grade.Element("Score") != null)
				{
					feedbackElement.Add(new XElement("score", grade.Element("Score").Value));
				}
		}
	}
	
	
	
	assessment.Add(feedbackElement);
	Console.WriteLine(assessment);
	Console.WriteLine(XElement.Parse(assessmentXml));
	
	
}



// Define other methods and classes here