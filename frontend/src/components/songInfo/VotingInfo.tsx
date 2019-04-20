import React from 'react';
import { Translate } from 'react-localize-redux';

interface VotingInfoProps {
    voteCount: number;
    voteCountChange?: number;
}

export const VotingInfo = (props: VotingInfoProps) => {
    return (
        <div className="voting-info">
            <span>
                {props.voteCount}&nbsp;
                {
                    props.voteCount === 1
                        ? <Translate id="vote" /> 
                        : <Translate id="votes" />
                }
                {
                    props.voteCountChange != null && props.voteCountChange != 0
                        ? <span className="badge">{voteCountChangeToString(props.voteCountChange)}</span>
                        : null
                }
            </span>
        </div>
    );
}

function voteCountChangeToString(voteCountChange: number) {
    if (voteCountChange > 0) {
        return "+" + voteCountChange;
    }

    return "" + voteCountChange;
}

export default VotingInfo;