<Query Kind="Program" />

void Main()
{
	string questionXml = "<Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"5\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"true\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	// If the assessment xml has "Scoring" tag in it, then set this variable to true
	bool scoringEnabled = true;
	
	// This value needs to be taken from the assesment xml. The attribute "ScoringMethod" can be found in the element named "Scoring"
	string scoringMethod = "any";
	
	string responseXml = "<Question ID=\"1\"><SelectedChoices><Choice><ID>1</ID></Choice><Choice><ID>2</ID></Choice></SelectedChoices></Question>";
	
	XElement question = XElement.Parse(questionXml);
	XElement response = XElement.Parse(responseXml);
	
	string scoringAt = question.Attribute("ScoringAt").Value;
	
	// If the correct choice has been selected by the user, then this score value will be given to the user 
	// for that particular question. This is only one case. In other cases, we need to consider the 
	// scoring method, if the type of the choice is checkbox
	int score = Convert.ToInt32(question.Attribute("Score").Value);
	
	string choiceType = question.Attribute("ChoiceType").Value;
	
	
	// Assuming ScoringAt is "question" & Choice Type is "checkboxes" &  Scoring Method is "any"
	
	// choice Ids the user has selected for this question
	List<int> userSelectedChoiceIds = new List<int>();
	
	userSelectedChoiceIds = response.Element("SelectedChoices").Elements("Choice").Select(item => Convert.ToInt32(item.Element("ID").Value)).ToList();
	
	// Here is the answers for this questions
	List<int> correctChoiceIds = new List<int>();
	
	correctChoiceIds = question.Elements("Choice")
							.Where(item => item.Attribute("Correct").Value == "true")
							.Select(item => Convert.ToInt32(item.Attribute("ID").Value)).ToList();
					
	decimal obtainedScore = 0.0m; bool success = true;
	
	Console.WriteLine(userSelectedChoiceIds); Console.WriteLine(correctChoiceIds);
	
	decimal totalNoofCorrectChoices = (decimal)correctChoiceIds.Count;
	
	decimal totalScore = (decimal)score;
	
	decimal scorePerCorrectChoice = (totalScore / totalNoofCorrectChoices);
	
	// #1. Checking if the no of given correct choices is equal
	if(userSelectedChoiceIds.Count <= correctChoiceIds.Count)
	{
		foreach(int uscid in userSelectedChoiceIds)
		{
			bool isCorrect = correctChoiceIds.IndexOf(uscid) != -1;
			
			if(isCorrect) obtainedScore += scorePerCorrectChoice;
			else { obtainedScore = 0.0m; success = false; break; }
		}
	}
	else
	{
		success = false;
	}

	Console.WriteLine(Math.Round(obtainedScore, 2));
}

// Define other methods and classes here
