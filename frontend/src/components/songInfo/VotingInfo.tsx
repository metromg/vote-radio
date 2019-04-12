import React from 'react';
import { Translate } from 'react-localize-redux';

interface VotingInfoProps {
    voteCount: number;
}

const VotingInfo = (props: VotingInfoProps) => {
    return (
        <div className="voting-info">
            <span>
                {props.voteCount}&nbsp;
                {
                    props.voteCount === 1
                        ? <Translate id="vote" /> 
                        : <Translate id="votes" />
                }
            </span>
        </div>
    );
}

export default VotingInfo;