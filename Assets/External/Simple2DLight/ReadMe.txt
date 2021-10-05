License : https://creativecommons.org/licenses/by/1.0/legalcode

MyNickname : Mirusona

Um.. It's Ok for If you don't write my nickname on your credit!


------------------------------------ Functions ----------------------------------------

1. CreateNormalLight : You can create normal light.
- position : Vector2
- size : float(0f ~ 2f)
- bright : float(0f ~ 2f)

1-1. CreateNormalLight : You can create normal light and index.
- index : int variable(Please initialize this variable) -> and add "ref" keyword if you put variable in parameter
- position : Vector2
- size : float(0f ~ 2f)
- bright : float(0f ~ 2f)

2. UpdateMoveLight : You can update normal light with this function.(Need index)
- index : index that you created
- position : Vector2
- size : float(0f ~ 2f)
- bright : float(0f ~ 2f)

3. DeleteNormalLight : You can delete normal light with this function.(Need index)
- index : index that you created

4. CreateFadeLight : You can create light that will grow up and down
- position : Vector2
- size : float(0f ~ 2f)
- speed : float(0.001f ~ 1f) -> grow up and down speed
- bright : float(0f ~ 2f)

------------------------------------ How To Use ----------------------------------------

*Please just see my example scripts.
-> NormalLightEX
-> FadeLightEX
-> MoveLightEX
-> LightBall

*Thanks