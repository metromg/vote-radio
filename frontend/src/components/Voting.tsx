import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { AppState } from '../store';
import { VotingCandidate } from '../store/voting/types';
import { selectVotingCandidate } from '../store/voting/actions';

interface VotingProps {
    candidates: VotingCandidate[];
    selectedSongId: string | null;

    selectVotingCandidate: (songId: string) => void;
}

class Voting extends Component<VotingProps> {
    render() {
        return (
            <ul>
                {this.props.candidates.map(candidate => {
                    const isSelected = candidate.songId === this.props.selectedSongId;
                    const style = isSelected
                        ? { background: "red" }
                        : undefined;

                    return (
                        <li key={candidate.songId} style={style}>
                            {candidate.title} {candidate.voteCount}
                            {
                                <button onClick={() => this.props.selectVotingCandidate(candidate.songId)} disabled={!candidate.isActive}>Vote</button>
                            }
                        </li>
                    );
                })}
            </ul>
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