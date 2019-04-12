import React, { Component } from 'react';

import { VotingCandidate } from '../../store/voting/types';
import SongDescription from '../songInfo/SongDescription';
import VotingInfo from '../songInfo/VotingInfo';
import CoverImage from '../songInfo/CoverImage';
import { apiBaseUrl } from '../../config';

interface CandidateProps {
    candidate: VotingCandidate;
    selected: boolean;

    onClick: () => void;
}

class Candidate extends Component<CandidateProps> {
    render() {
        const className = "voting-candidate" 
            + (!this.props.candidate.isActive ? " disabled" : "") 
            + (this.props.selected ? " selected" : "");

        const coverImageUrl = this.props.candidate.coverImageId != null
            ? apiBaseUrl + "/api/image/getImageAsync?imageId=" + this.props.candidate.coverImageId
            : null;

        return (
            <div className={className} tabIndex={0} role="button"
                 onClick={() => this.onClick()}
                 onKeyPress={e => this.onKeyPress(e)}>
                <VotingInfo voteCount={this.props.candidate.voteCount} />
                <CoverImage url={coverImageUrl} />
                <SongDescription 
                    title={this.props.candidate.title} 
                    album={this.props.candidate.album} 
                    artist={this.props.candidate.artist} 
                />
            </div>
        );
    }

    private onClick() {
        if (this.props.candidate.isActive) {
            this.props.onClick();
        }
    }

    private onKeyPress(e: React.KeyboardEvent) {
        const keyCode = e.which || e.keyCode;

        // Enter or Spacebar
        if (keyCode == 13 || keyCode == 32) {
            this.onClick();
        }
    }
}

export default Candidate;