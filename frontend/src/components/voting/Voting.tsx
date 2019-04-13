import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { AppState } from '../../store';
import { VotingCandidate } from '../../store/voting/types';
import { selectVotingCandidate } from '../../store/voting/actions';
import Candidate from './Candidate';
import './Voting.css';

interface VotingProps {
    candidates: VotingCandidate[];
    selectedSongId: string | null;

    selectVotingCandidate: (songId: string) => void;
}

class Voting extends Component<VotingProps> {
    render() {
        return (
            <div className="voting-container">
                {this.props.candidates.map(candidate => {
                    const selected = candidate.songId == this.props.selectedSongId;

                    return <Candidate 
                        key={candidate.songId}
                        candidate={candidate}
                        selected={selected}
                        onClick={() => this.props.selectVotingCandidate(candidate.songId)}
                    />
                })}
            </div>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    candidates: state.voting.candidates,
    selectedSongId: state.voting.selectedSongId
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    selectVotingCandidate: (songId: string) => dispatch<any>(selectVotingCandidate(songId))
});

export default connect(mapStateToProps, mapDispatchToProps)(Voting);