# StarshipSingularSyatem
Elton John Daries
Assessment Project.  In partial fullfillment of the requirements for Senior Software Engineer at Singular Systems
Singular Systems Programming Challenge - Starship Captain

You are the captain of a starship. You have been tasked with finding and colonizing habitable planets in your galaxy. 
Your home world is located at these coordinates: 123.123.99.1 X & 098.098.11.1 Y & 456.456.99.9 Z
Your coordinate system ranges from 000.000.00.0 to 999.999.99.9, and you live in a three-dimensional universe.

0.	Design and code an algorithm to generate a new universe within the bounds of the coordinate system.

1.	Design and code an algorithm to populate your universe, by randomly generating 15 000 locations that reside at a coordinate. Each of these locations must be randomly marked as a planet or a very hungry space monster. (Space monsters eat starships and should be avoided at all costs)
- Each of these coordinates marked as a planet must be randomly marked as habitable or not. 
- Each Habitable planet must have a surface area randomly ranging from 1 million to 100 million square kilometers. 
- In order to inhabit a planet, you have to colonize more than 50% of its surface.
- Colonization takes place at exactly 0.043 seconds per square kilometer.
- Due to the strange construction of the star system you live in, travel time between any planet and its immediate neighbor is always 10 minutes, except if the neighbor is a space monster, in which case travel time doubles because you have to dodge the monster.

Write the results to a .txt file in a format of your choosing. (Please include an explanation of the file format)

2.	Design and code an algorithm to read the file created in step 1 which will:
a.	Identify the planets you have to travel to and colonize to achieve the largest amount of colonized space within a 24-hour period. 
b.	Optimize your flight plan to avoid space monsters and non-habitable planets, visiting the largest amount of habitable planets, that you are able to colonize, in the 24 hour period 

3.	Create a report (on-screen or file) that explains the flight plan, which route was taken, and how much surface area was colonized in the 24 hour period.
