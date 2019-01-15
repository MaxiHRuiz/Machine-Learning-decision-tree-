using System;
using System.Collections.Generic;
namespace TrabajoIntegrador
{
	public class Particionador
	{
		public Particionador()
		{
		}
		
		public static Dictionary<string, int> class_counts(IList<IList<string>> rows){
			//Counts the number of each type of example in a dataset."""
			Dictionary<string, int> counts = new Dictionary<string, int>();  // a dictionary of label -> count.
			foreach (var row in rows) {
				// in our dataset format, the label is always the last column
				string label = row[row.Count-1];
				if(!counts.ContainsKey(label)){
					counts.Add(label, 0);
				}
				counts[label] = counts[label]+1;
			}
			return counts;
		}
		
		private static double gini(IList<IList<string>>  rows){
			/* Calculate the Gini Impurity for a list of rows.
		    There are a few different ways to do this, I thought this one was
		    the most concise. See:    https://en.wikipedia.org/wiki/Decision_tree_learning#Gini_impurity */
			Dictionary<string, int> counts = class_counts(rows);
			double impurity = 1.0;
			foreach (var pair in counts)
			{
				double prob_of_lbl = (double) pair.Value / (double) rows.Count;
				impurity -= Math.Pow(prob_of_lbl, 2.0);
			}
			
			return impurity;
		}
		
		
		public static IList<IList<IList<string>>> partition(IList<IList<string>> rows, Pregunta question){
			//Partitions a dataset.
			// For each row in the dataset, check if it matches the question. If
			//  so, add it to 'true rows', otherwise, add it to 'false rows'.
			
			List<IList<string>>   ltrue_rows = new List<IList<string>>();
			List<IList<string>>  lfalse_rows = new List<IList<string>>();
			
			foreach (var row in rows) {
				if (question.coincide(row))
					ltrue_rows.Add(row);
				else
					lfalse_rows.Add(row);
			}

			IList<IList<IList<string>>> res = (IList<IList<IList<string>>>) new List<IList<IList<string>>>();
			res.Add(ltrue_rows);
			res.Add(lfalse_rows);
			return res;
		}
		
		private static double info_gain(IList<IList<string>> left, IList<IList<string>> right, double current_uncertainty){
			//Information Gain.
			// The uncertainty of the starting node, minus the weighted impurity of
			// two child nodes.
			double p = (double) left.Count / ((double) left.Count + (double) right.Count);
			return current_uncertainty - p * gini(left) - (1.0 - p) * gini(right);
		}
		
		
		public static Dictionary<Pregunta, double> find_best_split(IList<IList<string>>  rows, IList<string> header){
			//Find the best question to ask by iterating over every feature / value
			//and calculating the information gain.
			
			double best_gain = 0;  // keep track of the best information gain
			Pregunta best_question= null; //  # keep train of the feature / value that produced it
			double current_uncertainty = gini(rows);
			int n_features = header.Count - 1;  // number of columns
			List<string> values ;
			for( int col=0; col<n_features; col++){  // for each feature

				values = new List<string>();  // unique values in the column
				for (int i = 0; i < rows.Count; i++) {
					if (!values.Contains(rows[i][col])) {
						values.Add(rows[i][col]);
					}
				}
				foreach (var val in values) {//for each value
					
					
					Pregunta question = new Pregunta(col, val, header[col]);

					// try splitting the dataset
					IList<IList<IList<string>>> res= (IList<IList<IList<string>>>) partition(rows, question);
					IList<IList<string>>  true_rows=res[0];
					IList<IList<string>>  false_rows =res[1];

					// Skip this split if it doesn't divide the dataset.
					if (true_rows.Count == 0 || false_rows.Count  == 0) continue;

					// Calculate the information gain from this split
					double gain = info_gain(true_rows, false_rows, current_uncertainty);
					//Console.WriteLine("P {1} gain: {0}", gain, question);
					// You actually can use '>' instead of '>=' here
					// but I wanted the tree to look a certain way for our toy dataset.
					if (gain >= best_gain){
						best_gain = gain;
						best_question = question;
					}
				}
			}
			
			Dictionary<Pregunta, double> result = new Dictionary<Pregunta, double>();
			if (best_question == null) {
				result.Add(new Pregunta(0, "", ""),best_gain);
			}else
				result.Add(best_question,best_gain);
			return result;
		}
	}
}
