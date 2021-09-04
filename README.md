# Educational-Solar-Sim
Educational application that takes a drawn or provided rough image of a solar system and provides a simulation of how such a system would behave using Newtonian physics.

## Coding Standards 
1. The naming convetion for functions, methods, variables should follow cammelCase.
2. Method/Function naming convenction 
    - Must start with a verb 
    - Keep word description minimal 
        - Roughly 1-3 words, but not a hard restriction 
    - Variables primarily should be longer than 2 characters
        - Iteratiors, for example, are exempt 
    - Variables should be descriptive of what they are
3. Objects and Classes 
    - Both follow PascalCase naming convention 
    - C++ compiles top to bottom, classes and code should be in hierarchy format 
        - Example: If methods A requires method B, method B is above methods A in the code
    - Use as much modularization as possible, i.e. minimize method line sizes to a simplification extent
4. Formatting and Indentation
    - Curly braces are on a line of their own **no matter what**
        - This includes methods, classes, if statements, loops, etc.
    - **Tab indentation is required**
5. Commenting and Documenting
    - Method Comments 
    ```
	/**
	* Method parameter meaning and purpose (description of all inputs)
	* Method functionality & purpose
	* Output description
	**/
    ```
    - All Others 
        ```
        // Should be used for variable & function calls/operations if needed the line above the operation 
        ```
6. Branches & Features 
    - Features should be branched off of the develop branch 
    - Naming convention for features should be feature-[featureName]
        - Feature name should be cammelCase
    - Once feature is completed should be merged back to develop, where develop is eventually merged to main 
        - To merge to develop it requires two reviewers to approve
        - To merge to main it requires six reviewers to approve 
7. Testing 
    - At the moment, you may test however you please 