<Query Kind="Program" />

void Main()
{
	string questionXml = "<Question ID=\"3\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"1\" ChoiceType=\"singleline\"><Title>4 + 3  = ?</Title><Choice ID=\"1\"><FieldFormat>number</FieldFormat><MaximumLength>2</MaximumLength></Choice></Question>";
	
	// If the assessment xml has "Scoring" tag in it, then set this variable to true
	bool scoringEnabled = true;
	
	// This value needs to be taken from the assesment xml. The attribute "ScoringMethod" can be found in the element named "Scoring"
	string scoringMethod = "any";
	
	string responseXml = "<Question ID=\"3\"><ResponseText><![CDATA[7]]></ResponseText></Question>";
	
	XElement question = XElement.Parse(questionXml);
	XElement response = XElement.Parse(responseXml);
	
	string scoringAt = question.Attribute("ScoringAt").Value;
	
	// If the correct choice has been selected by the user, then this score value will be given to the user 
	// for that particular question. This is only one case. In other cases, we need to consider the 
	// scoring method, if the type of the choice is checkbox
	int score = Convert.ToInt32(question.Attribute("Score").Value);
	
	string choiceType = question.Attribute("ChoiceType").Value;
	

					
	decimal obtainedScore = 0.0m; bool success = true;
	
	// success is, if the user answers this question itself he will get the score
	
	string responseText = response.Element("ResponseText").Value;
	
	success = !string.IsNullOrEmpty(responseText);
	
	if(success) obtainedScore = (decimal)score;
	

	Console.WriteLine(Math.Round(obtainedScore, 2));
}

// Define other methods and classes here