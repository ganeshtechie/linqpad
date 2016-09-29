<Query Kind="Program" />

void Main()
{
	
	string questionXml = "<Question ID=\"1\" Rhetorical=\"true\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"20\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	string responseXml = "<Question ID=\"1\"><SelectedChoices><Choice><ID>2</ID></Choice><Choice><ID>1</ID></Choice></SelectedChoices></Question>";
	
	string scoreSheetXml = "<Question ID=\"1\" IsAnswered=\"true\"><Score>1.3</Score></Question>";
	
	
	XElement question = XElement.Parse(questionXml);
	
	XElement response = XElement.Parse(responseXml);
	
	XElement scoreSheet = XElement.Parse(scoreSheetXml);
	
	// If the "Rhetorical" element is not found in the xml, conside the question type as non rhetorical
	bool rhetoricalQuestion = question.Attribute("Rhetorical") != null && question.Attribute("Rhetorical").Value == "true";
	
	//Console.WriteLine(response);
	
	//Question
	//Your Answer  - Choice Text / No Response
	//Score - Score got for each question. For rhetorical questions, scores won't be there. But the user might have answered it
	
	XElement reportQuestion = new XElement("Question");
	
	string title = question.Element("Title").Value;
	string yourAnswer = string.Empty;
	
	
	// Adding the Question title to the report object
	reportQuestion.Add(new XElement("Title", new XCData(title)));
	
	// I assume that, all the questions here are choice based
	
	if(response == null)
	{
		// We need to generate report for question which are unanswered by the user
		// Because its a report. you've to put everything in to it.
		reportQuestion.Add(new XElement("YourAnswer", null));
	}
	else
	{
		List<int> choiceIds = new List<int>();
	
		choiceIds = response.Element("SelectedChoices").Elements("Choice")
					.Select(_choice => Convert.ToInt32(_choice.Element("ID").Value)).ToList();
					
		List<string> choiceText = new List<string>();
	
		foreach(int cid in choiceIds)
		{
			string _choicetext = question.Elements("Choice")
							.Where(_choice => _choice.Attribute("ID").Value == cid.ToString())
							.Select(_choice => _choice.Element("Title").Value).FirstOrDefault();
							
			choiceText.Add(_choicetext);
		}
		
		
		foreach(string ct in choiceText)
		{
			reportQuestion.Add(new XElement("YourAnswer", new XCData(ct)));
		}
	}
	
	
	
	// If score sheet is passed, add scores to it
	if(scoreSheet != null)
	{
		decimal score = Convert.ToDecimal(scoreSheet.Element("Score").Value);
		
		if(rhetoricalQuestion) reportQuestion.Add(new XElement("Rhetorical", true));
		else reportQuestion.Add(new XElement("Score", score));
	}
	
	Console.WriteLine(reportQuestion);
	
}

// Define other methods and classes here
