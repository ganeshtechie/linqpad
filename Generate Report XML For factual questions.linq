<Query Kind="Program" />

void Main()
{
	
	string questionXml = "<Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"20\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	string responseXml = "<Question ID=\"1\"><ResponseText>Awesome! I can put whatever i want here</ResponseText></Question>";
	
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
		string responseText = response.Element("ResponseText").Value;
		reportQuestion.Add(new XElement("YourAnswer", new XCData(responseText)));
		
	}
	
	
	// If score sheet is passed, add scores to it
	if(scoreSheet != null)
	{
		// For Factual Questions, scoring is a confusing thing. I feel that we can still scores for 
		// Factual questions ( obviously, on question level ). So when user answers it, 
		// We are gonna give the complete score to the participant. No need to 
		// check if the response is a correct response. Yeah. Its a Factual Question
		decimal score = Convert.ToDecimal(scoreSheet.Element("Score").Value);
		
		if(rhetoricalQuestion) reportQuestion.Add(new XElement("Rhetorical", true));
		else reportQuestion.Add(new XElement("Score", score));
	}
	
	Console.WriteLine(reportQuestion);
	
}

// Define other methods and classes here