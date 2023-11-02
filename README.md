# dialogs_management

The table of contents of this repository can be found as a PNG in the base folder. The following files are excluded from that table:
 - A commented UML diagram showing how the inspector sections are structured (following a composite pattern).
 - The _screenshots folder, which contains several images of an example inspector.
 - The table of contents itself.
 - This README document, which includes some documentation of the main classes in this repository.


DIALOGS MANAGER - QUICK DOCUMENTATION

 - The DialogsManager will retrieve a dialog from a CSV document based on the specified language and a given keyword, assigning it to a previously chosen TMP_Text.
 - The dialogs will be displayed at once or with a span time between characters, if such delay has been previously determined.
 - There is a general delay time for all characters and dialogs, but that value can be overrided (for characters) or adjusted (for dialogs) on a case by case basis.

 - The DialogsManager has two public properties (Language, TextSpeed) and four public methods (GetDialog, GetAllDialogs, DisplayDialog, DisplayAllDialogs).
 - The Language property can be used to change the active language. (Please note that changing the language will not automatically call the method to update the dialogs to the new language.)
 - The TextSpeed property allows to modify the base delay between characters both in the Editor and at runtime. It acts as a multiplier and affects all the dialogs in the game.
 - The GetDialog method requires a keyword, and will return the dialog associated to that keyword (in the currently active language).
 - The GetAllDialogs method will retrieve all the dialogs in the scene as an array.
 - The DisplayDialog method will assign a dialog to a TMP_Text. It requires passing a keyword and (optionally) a dialog. When the latter is not provided, the method will use the one associated to the keyword (in the currently active language).
 - The DisplayAllDialogs method will assign all the dialogs to their respective TMP_Texts at once.


DIALOG VARIABLES MANAGER - QUICK DOCUMENTATION

 - The DialogVariablesManager stores the name of the given variables and their values in a dictionary, and allows to get, add, update or remove them at will.
 - The DialogVariablesManager requires two strings as parameters in the constructor: one for the characters marking the start of the variable and another one for the characters marking the end.
 - The opening and closing characters of the variables must be different fron each other.

 - The DialogVariablesManager has five public methods: InsertVariablesInDialog, GetVariableValue, AddVariable, UpdateVariable and RemoveVariable.
 - The InsertVariablesInDialog method requires a dialog as a parameter, and will replace by their actual values all the parts of that dialog marked as variables.
 - The AddVariable and UpdateVariable methods require passing both the name of the variable (without the opening and closing characters) and the new value, while the GetVariableValue and RemoveVariable methods only require the name.


DIALOGS MANAGER CUSTOM INSPECTOR - QUICK DOCUMENTATION

 - CSV document. Loads an existing CSV document or creates a new one.
 - Row for language names. The number of the row that the dialogs manager will examine in search of the current language (starting from 1).
 - Column for keywords. The number of the column where the dialogs manager will look for the specified keywords (starting from 1).

 - Load document. Loads the selected CSV document into the inspector, so that it can be edited from there. (Note that this is mainly thought for quick fixes or additions, and not to edit the document from scratch - although it is possible to do so.)

 - Rows. Shows the row that is currently displayed, along with the total number of rows in the document and a couple of navigation arrows.
 - Remove row. Removes the row that is currently displayed.
 - Add row. Adds a new row and makes it the currently displayed one.

 - {Left column}. Shows the number and "title" (retrieved from the row for language names) of the currently displayed column, as well as its content and two navigation arrows.
 - {Right column}. Same as above. The reason for having two columns is to facilitate comparisons between different sets of data (a keyword and its correspondent dialog, the original text and its translation in a certain language, etc.).
 - Remove column. Removes the column above.

 - Columns. Shows the total number of columns in the document.
 - Add column. Adds a new column and displays it in the right half of the inspector.

 - SAVE CHANGES (IMPORTANT!). Overwrites the document with all the changes that have been made in the inspector. (Note: adding or removing a row or column will automatically save any other changes).
 - Unload document. Unloads the CSV document.

 - Add new keywords from document. Compares the keywords in the document to those in the inspector and completes the latter with the ones that is lacking (if any).

 - TMP_Text. A list of all the texts that are linked to keywords.
 - Keyword. A list of all the keywords that are linked to TMP_Texts.
 - Delay multiplier. A list of all the multipliers connected to the texts and their dialogs. These multipliers will modify the base delay value of the characters in the dialogs connected to them. (This field will be hidden if the character configuration is too.)
 - Add new. Adds a new connection (one TMP_Text, one keyword and one delay multiplier).

 - SHOW CHARACTER CONFIGURATION. Shows the options that allow to add or modify span times between characters (including the delay multiplier above).
 - HIDE CHARACTER CONFIGURATION. Hides those options.

 - Default delay. Allows to set the base delay for all the characters that are not marked as exceptions (when set to 0, the dialogs will appear at once, with no delay between characters).
 - Add exceptions. Adds new exceptions to the base delay value. All the characters added as exceptions and their respective span times are displayed as a table right below this button.

 - Min. Sets the minimum value for the runtime speed multiplier.
 - Max. Sets the maximum value for the runtime speed multiplier.
 - Current. Sets the current value for the runtime speed multiplier. This value allows the player to change the speed at which the texts appear on screen, by multiplying their respective delays (it can be set to zero in order to display them at once).
