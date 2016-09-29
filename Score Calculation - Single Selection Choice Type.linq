<Query Kind="Program" />

void Main()
{
	string questionXml = "<Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"5\" ChoiceType=\"radiobutton\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question>";
	
	// If the assessment xml has "Scoring" tag in it, then set this variable to true
	bool scoringEnabled = true;
	
	// This value needs to be taken from the assesment xml. The attribute "ScoringMethod" can be found in the element named "Scoring"
	string scoringMethod = "any";
	
	string responseXml = "<Question ID=\"1\"><SelectedChoices><Choice><ID>3</ID></Choice></SelectedChoices></Question>";
	
	XElement question = XElement.Parse(questionXml);
	XElement response = XElement.Parse(responseXml);
	
	string scoringAt = question.Attribute("ScoringAt").Value;
	
	// If the correct choice has been selected by the user, then this score value will be given to the user 
	// for that particular question. This is only one case. In other cases, we need to consider the 
	// scoring method, if the type of the choice is checkbox
	int score = Convert.ToInt32(question.Attribute("Score").Value);
	
	string choiceType = question.Attribute("ChoiceType").Value;
	
	
	// Assuming ScoringAt is "question" & Choice Type is "checkboxes" &  Scoring Method is "any"
	
	// choice Id the user has selected for this question
	int userSelectedChoiceId;
	
	userSelectedChoiceId = response.Element("SelectedChoices").Elements("Choice").Select(item => Convert.ToInt32(item.Element("ID").Value)).FirstOrDefault();
	
	// Here is the answers for this questions
	List<int> correctChoiceIds = new List<int>();
	
	correctChoiceIds = question.Elements("Choice")
							.Where(item => item.Attribute("Correct").Value == "true")
							.Select(item => Convert.ToInt32(item.Attribute("ID").Value)).ToList();
					
	decimal obtainedScore = 0.0m; bool success = true;
	
	success = correctChoiceIds.IndexOf(userSelectedChoiceId) != -1;
	
	if(success) obtainedScore = (decimal)score;
	

	Console.WriteLine(Math.Round(obtainedScore, 2));
}

// Define other methods and classes here
