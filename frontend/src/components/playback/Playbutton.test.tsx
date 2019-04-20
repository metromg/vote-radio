import React from 'react';
import { shallow, mount } from 'enzyme';
import { Playbutton } from './Playbutton';

describe('Playbutton', () => {
    it('renders without crashing', () => {
        shallow(<Playbutton playing={false} loading={false} onClick={() => null} />);
    });

    describe('when playing', () => {
        it('renders stop symbol', () => {
            // Arrange
            const element = <Playbutton playing={true} loading={false} onClick={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain('icons8-stop-filled-50.png');
            expect(result.html()).not.toContain('icons8-play-filled-50.png');
            expect(result.html()).not.toContain('lds-spinner');
        });

        it('handles click event', () => {
            // Arrange
            const onClick = jest.fn();
            const element = <Playbutton playing={true} loading={false} onClick={onClick} />;

            // Act
            const result = mount(element);
            
            result.simulate('click');

            // Assert
            expect(onClick).toBeCalledTimes(1);
        });
    });

    describe('when playing and loading', () => {
        it('renders loading spinner', () => {
            // Arrange
            const element = <Playbutton playing={true} loading={true} onClick={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain('lds-spinner');
            expect(result.html()).not.toContain('icons8-stop-filled-50.png');
            expect(result.html()).not.toContain('icons8-play-filled-50.png');
        });

        it('does not handle click event', () => {
            // Arrange
            const onClick = jest.fn();
            const element = <Playbutton playing={true} loading={true} onClick={onClick} />;

            // Act
            const result = mount(element);
            
            result.simulate('click');

            // Assert
            expect(onClick).toBeCalledTimes(0);
        });
    });

    describe('when stopped', () => {
        it('renders play symbol', () => {
            // Arrange
            const element = <Playbutton playing={false} loading={false} onClick={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain('icons8-play-filled-50.png');
            expect(result.html()).not.toContain('icons8-stop-filled-50.png');
            expect(result.html()).not.toContain('lds-spinner');
        });

        it('handles click event', () => {
            // Arrange
            const onClick = jest.fn();
            const element = <Playbutton playing={false} loading={false} onClick={onClick} />;

            // Act
            const result = mount(element);
            
            result.simulate('click');

            // Assert
            expect(onClick).toBeCalledTimes(1);
        });
    });

    describe('when stopped and loading', () => {
        it('renders loading spinner', () => {
            // Arrange
            const element = <Playbutton playing={false} loading={true} onClick={() => null} />;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain('lds-spinner');
            expect(result.html()).not.toContain('icons8-stop-filled-50.png');
            expect(result.html()).not.toContain('icons8-play-filled-50.png');
        });

        it('does not handle click event', () => {
            // Arrange
            const onClick = jest.fn();
            const element = <Playbutton playing={false} loading={true} onClick={onClick} />;

            // Act
            const result = mount(element);
            
            result.simulate('click');

            // Assert
            expect(onClick).toBeCalledTimes(0);
        });
    });
});