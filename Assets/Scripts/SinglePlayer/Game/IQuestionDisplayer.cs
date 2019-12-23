using System;

public interface IQuestionDisplayer
{
    event EventHandler<OnRoundFinishEventArgs> OnRoundFinish;
    event EventHandler<OnCheckAnswerEventArgs> OnCheckAnswer;
}