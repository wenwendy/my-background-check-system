﻿using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public class BobInvitationTransformer : IInvitationTransformer
    {
        public object Transform(InvitationDto invitation)
        {
            return invitation;
        }

    }
}